using Mapster;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Asn1.Ocsp;
using Review_Web_App.Entities;
using Review_Web_App.Models.DTOs;
using Review_Web_App.Repositories.Implementations;
using Review_Web_App.Repositories.Interfaces;
using Review_Web_App.Services.Interfaces;

namespace Review_Web_App.Services.Implementations
{
    public class BookmarkService : IBookmarkService
    {
        private readonly IBookmarkRepository _bookmarkRepository;
        private readonly IReviewerService _reviewerService;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public BookmarkService(IBookmarkRepository bookmarkRepository, IUserService userService, IReviewerService reviewerService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _reviewerService = reviewerService;
            _userService = userService;
            _bookmarkRepository = bookmarkRepository;
        }

        public async Task<bool> Check(Guid postId, Guid reviewerId)
        {
            var check = await _bookmarkRepository.Check(x => x.PostId == postId && x.ReviewerId == reviewerId);
            if (check)
            {
                return true;
            }
            return false;
        }

        public async Task<BaseResponse<BookmarkResponseModel>> CreateBookmark(BookmarkRequestModel request)
        {
            var check = await _bookmarkRepository.Check(x => x.PostId == request.PostId && x.ReviewerId == request.ReviewerId);
            if (check)
            {
                return new BaseResponse<BookmarkResponseModel>
                {
                    Message = "BookMark Already Exists"
                };
            }
            var bookmark = new Bookmark
            {
                PostId = request.PostId,
                ReviewerId = request.ReviewerId
            };
            await _bookmarkRepository.Create(bookmark);
            await _unitOfWork.SaveWork();
            return new BaseResponse<BookmarkResponseModel>
            {
                Data = bookmark.Adapt<BookmarkResponseModel>(),
                Message = "Bookmark Added",
                Success = true
            };
        }

        public async Task<BaseResponse<BookmarkResponseModel>> DeleteBookmark(BookmarkRequestModel request)
        {
            var getBookmark = await _bookmarkRepository.Get(x => x.ReviewerId == request.ReviewerId && x.PostId == request.PostId);
            if (getBookmark != null)
            {
                _bookmarkRepository.Delete(getBookmark);
                await _unitOfWork.SaveWork();
                return new BaseResponse<BookmarkResponseModel>
                {
                    Message = "Deleted Successfully",
                    Success = true
                };
            }
            return new BaseResponse<BookmarkResponseModel>
            {
                Message = "Not Found",
            };
        }

        public async Task<BaseResponse<ICollection<BookmarkResponseModel>>> GetAllBookmarks()
        {
            var getAllBookmarks = await _bookmarkRepository.GetAll();
            if (getAllBookmarks.Count == 0)
            {
                return new BaseResponse<ICollection<BookmarkResponseModel>>
                {
                    Message = "No Bookmark found"
                };
            }
            return new BaseResponse<ICollection<BookmarkResponseModel>>
            {
                Data = getAllBookmarks.Select(x => x.Adapt<BookmarkResponseModel>()).ToList(),
                Message = "All Bookmarks",
                Success = true
            };

        }

        public async Task<BaseResponse<ICollection<PostResponseModel>>> GetReviewerBookmark(Guid reviewerId)
        {
            var getBookmarkByReviewer = await _bookmarkRepository.GetReviewerBookmark(reviewerId);
            if (getBookmarkByReviewer.Count() == 0)
            {
                return new BaseResponse<ICollection<PostResponseModel>>
                {
                    Message = "No Bookmark Added"
                };

            }
            var postResponseList = new List<PostResponseModel>();
            foreach (var bookmark in getBookmarkByReviewer.Where(x => x.IsDeleted == false))
            {
                    var reviewerResponse = await _reviewerService.GetReviewer(bookmark.ReviewerId);
                    BaseResponse<UserResponse> userResponse = await _userService.GetUser(reviewerResponse.Data.UserId);
                    var reviewerProfilePicture = reviewerResponse?.Data.ProfilePicture;
                    var reviewerUsername = userResponse.Data.UserName;
                    var postResponse = new PostResponseModel
                    {
                        PostTitle = bookmark.PostTitle,
                        PostText = bookmark.PostText,
                        Id = bookmark.Id,
                        PostFile = bookmark.PostFile,
                        ReviewerProfilePicture = reviewerProfilePicture,
                        RevieweUsername = reviewerUsername
                    };

                    postResponseList.Add(postResponse);
            }
            return new BaseResponse<ICollection<PostResponseModel>>
            {
                Data = postResponseList,
                Message = "All bookmarks",
                Success = true
            };
        }
    }
}
     

