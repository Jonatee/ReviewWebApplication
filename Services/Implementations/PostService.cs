using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NuGet.Protocol;
using Review_Web_App.Context;
using Review_Web_App.Entities;
using Review_Web_App.Models.DTOs;
using Review_Web_App.Repositories.Implementations;
using Review_Web_App.Repositories.Interfaces;
using Review_Web_App.Services.Interfaces;

namespace Review_Web_App.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IReviewerService _reviewerService;
        private readonly IBookmarkService _bookmarkService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileRepository _fileRepository;
        private readonly IUserService _userService;

        public PostService(IPostRepository postRepository,IBookmarkService bookmarkService,IUserService userService,IReviewerService reviewerService, IUnitOfWork unitOfWork, IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
            _reviewerService = reviewerService;
            _userService = userService;
            _postRepository = postRepository;
            _bookmarkService = bookmarkService;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseResponse<PostResponseModel>> CreatePost(PostRequestModel request)
        {
            var reviewerId = await _reviewerService.GetReviewerByLoggedInUser();
            var post = new Post()
            {
              PostTitle = request.PostTitle,
              PostText = request.PostText,
              CategoryId = request.CategoryId,
              ReviewerId = reviewerId.Data.Id, 
              PostFile = await _fileRepository.UploadAsync(request.PostFile)
            };
            await _postRepository.Create(post);
            await _unitOfWork.SaveWork();

            return new BaseResponse<PostResponseModel>
            {
                 Data = new PostResponseModel
                 {
                     Id = post.Id,
                     PostText= post.PostText,
                     CategoryId = post.CategoryId,
                     PostFile = post.PostFile,
                     PostTitle = post.PostTitle,
                     ReviewerId = post.ReviewerId,
                     DateCreated = post.DateCreated
                 },
                 Message = "Post Created Successfully",
                 Success = true
            };
        }

        public async Task<BaseResponse<PostResponseModel>> DeletePost(Guid id)
        {
            var getPost = await _postRepository.Get(id);
            if (getPost == null)
            {
                return new BaseResponse<PostResponseModel>
                {
                    Message = "Post Not Found",
                };
            }
            getPost.IsDeleted = true;
            _postRepository.Update(getPost);
            await _unitOfWork.SaveWork();
            return new BaseResponse<PostResponseModel>
            {
                Data = getPost.Adapt<PostResponseModel>(),
                Message = "Post Found",
                Success = true
            };
        }

        public async Task<BaseResponse<ICollection<PostResponseModel>>> GetAllPosts()
        {
            var getAllPost = await _postRepository.GetAll();
            var response = await _reviewerService.GetReviewerByLoggedInUser();

            if (getAllPost.Count == 0 || getAllPost.All(x => x.IsDeleted))
            {
                return new BaseResponse<ICollection<PostResponseModel>>
                {
                    Message = "No Posts Found",
                    Success = false
                };
            }

            var postResponseList = new List<PostResponseModel>();

            foreach (var post in getAllPost.Where(x => !x.IsDeleted))
            {
                
                
                    var reviewerResponse = await _reviewerService.GetReviewer(post.ReviewerId);
                    BaseResponse<UserResponse> userResponse = await _userService.GetUser(reviewerResponse.Data.UserId);
                    var reviewerProfilePicture = reviewerResponse?.Data.ProfilePicture;
                    var reviewerUsername = userResponse?.Data.UserName;

                    
                    var postResponse = new PostResponseModel
                    {
                        Id = post.Id,
                        PostTitle = post.PostTitle,
                        PostFile = post.PostFile,
                        CategoryId = post.CategoryId,
                        PostText = post.PostText,
                        ReviewerProfilePicture = reviewerProfilePicture,
                        RevieweUsername = reviewerUsername,
                        DateCreated = post.DateCreated,
                        Likes = post.Likes.Select(l => new LikeResponseModel
                        {
                            Id = l.Id,
                            PostId = l.PostId,
                            ReviewerId = l.ReviewerId,
                        }).ToList(),
                        IsLikedByUser = post.Likes.Any(like => like.ReviewerId == response.Data?.Id)
                    };

                    postResponseList.Add(postResponse);
                
            }

            return new BaseResponse<ICollection<PostResponseModel>>
            {
                Data = postResponseList,
                Message = "All Posts",
                Success = true
            };
        }



        public async Task<BaseResponse<PostResponseModel>> GetPost(Guid Id)
        {
            var post = await _postRepository.Get(Id);
            var response = await _reviewerService.GetReviewerByLoggedInUser();
            if (post == null || post.IsDeleted == true)
            {
                return new BaseResponse<PostResponseModel>
                {
                    Message = "Post Not Found",
                };
            }
            var reviewerResponse = await _reviewerService.GetReviewer(post.ReviewerId);
            BaseResponse<UserResponse> userResponse = await _userService.GetUser(reviewerResponse.Data.UserId);
            var reviewerProfilePicture = reviewerResponse?.Data.ProfilePicture;
            var reviewerUsername = userResponse.Data?.UserName;
            var comments = new List<CommentResponseModel>();
            foreach (var comment in post.Comments)
            {
                var reviewer = await _reviewerService.GetReviewer(comment.ReviewerId);
                var user = await _userService.GetUser(reviewer.Data.UserId);
                comments.Add(new CommentResponseModel
                {
                    CommentText = comment.CommentText,
                    FileUrl = comment.FileUrl,
                    PostId = comment.PostId,
                    Id = comment.Id,
                    ReviewerUserName = user.Data.UserName,
                    DateCreated = comment.DateCreated,
                    ReviewerId = comment.ReviewerId,
                });
            }
            
            var responses =  new BaseResponse<PostResponseModel>
            {
                Data = new PostResponseModel
                {
                    Id = post.Id,
                    PostTitle = post.PostTitle,
                    PostFile = post.PostFile,
                    CategoryId = post.CategoryId,
                    PostText = post.PostText,
                    ReviewerProfilePicture = reviewerProfilePicture,
                    RevieweUsername = reviewerUsername,
                    DateCreated = post.DateCreated,
                    Likes = post.Likes.Select(l => new LikeResponseModel
                    {
                        Id = l.Id,
                        PostId = l.PostId,
                        ReviewerId = l.ReviewerId,
                    }).ToList(),
                    Comments =  comments,
                    IsLikedByUser = post.Likes.Any(like => like.ReviewerId == response.Data?.Id),
                    IsBookmarkedByUser = await _bookmarkService.Check(post.Id,response.Data.Id)
                    
                   
                },
                Message = "Post Found",
                Success = true

            };
            if(responses.Data is PostResponseModel postResponse)
            {
                postResponse.SetIsCommentByUser(response.Data.Id);
            }
            return responses;
        }

        public async Task<BaseResponse<ICollection<PostResponseModel>>> GetReviewerPosts()
        {
            var response = await _reviewerService.GetReviewerByLoggedInUser();
            var postByAReviewer = await _postRepository.GetPostsByReviewer(response.Data.Id);
            if(postByAReviewer.Count() == 0 || postByAReviewer.All(x =>x.IsDeleted == true))
            {
                return new BaseResponse<ICollection<PostResponseModel>>
                {
                    Message = "No Posts Found",
                    Success = false,
                };
            }
            var postResponseList = new List<PostResponseModel>();

            foreach (var post in postByAReviewer.Where(x => x.IsDeleted == false))
            {


                var reviewerResponse = await _reviewerService.GetReviewer(post.ReviewerId);
                BaseResponse<UserResponse> userResponse = await _userService.GetUser(reviewerResponse.Data.UserId);
                var reviewerProfilePicture = reviewerResponse?.Data.ProfilePicture;
                var reviewerUsername = userResponse?.Data.UserName;


                var postResponse = new PostResponseModel
                {
                    Id = post.Id,
                    PostTitle = post.PostTitle,
                    PostFile = post.PostFile,
                    CategoryId = post.CategoryId,
                    PostText = post.PostText,
                    ReviewerProfilePicture = reviewerProfilePicture,
                    RevieweUsername = reviewerUsername,
                    DateCreated = post.DateCreated,
                    Likes = post.Likes.Select(l => new LikeResponseModel
                    {
                        Id = l.Id,
                        PostId = l.PostId,
                        ReviewerId = l.ReviewerId,
                    }).ToList(),
                    IsLikedByUser = post.Likes.Any(like => like.ReviewerId == response.Data?.Id)
                };

                postResponseList.Add(postResponse);

            }
            return new BaseResponse<ICollection<PostResponseModel>>
            {
                Data = postResponseList,
                Success = true,
                Message = "Posts by User",
            };

        }

        public async Task<BaseResponse<ICollection<PostResponseModel>>> Search(string title, Guid? category)
        {
            var getAllPost = await _postRepository.SearchPosts(title,category);
            var response = await _reviewerService.GetReviewerByLoggedInUser();

            if (getAllPost.Count == 0 || getAllPost.All(x => x.IsDeleted == true))
            {
                return new BaseResponse<ICollection<PostResponseModel>>
                {
                    Message = "No Results Found",
                    Success = false
                };
            }

            var postResponseList = new List<PostResponseModel>();

            foreach (var post in getAllPost.Where(x => x.IsDeleted == false))
            {


                var reviewerResponse = await _reviewerService.GetReviewer(post.ReviewerId);
                BaseResponse<UserResponse> userResponse = await _userService.GetUser(reviewerResponse.Data.UserId);
                var reviewerProfilePicture = reviewerResponse?.Data.ProfilePicture;
                var reviewerUsername = userResponse?.Data.UserName;


                var postResponse = new PostResponseModel
                {
                    Id = post.Id,
                    PostTitle = post.PostTitle,
                    PostFile = post.PostFile,
                    CategoryId = post.CategoryId,
                    PostText = post.PostText,
                    ReviewerProfilePicture = reviewerProfilePicture,
                    RevieweUsername = reviewerUsername,
                    DateCreated = post.DateCreated,
                    Likes = post.Likes.Select(l => new LikeResponseModel
                    {
                        Id = l.Id,
                        PostId = l.PostId,
                        ReviewerId = l.ReviewerId,
                    }).ToList(),
                    IsLikedByUser = post.Likes.Any(like => like.ReviewerId == response.Data?.Id)
                };

                postResponseList.Add(postResponse);

            }

            return new BaseResponse<ICollection<PostResponseModel>>
            {
                Data = postResponseList,
                Message = "Searched Posts",
                Success = true
            };
        }
    }
}
