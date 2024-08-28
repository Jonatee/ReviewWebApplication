using Mapster;
using Review_Web_App.Constants;
using Review_Web_App.Context;
using Review_Web_App.Entities;
using Review_Web_App.Models.DTOs;
using Review_Web_App.Repositories.Implementations;
using Review_Web_App.Repositories.Interfaces;
using Review_Web_App.Services.Interfaces;
using System.Security.Claims;
using ZstdSharp.Unsafe;

namespace Review_Web_App.Services.Implementations
{
    public class ReviewerService : IReviewerService
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IFileRepository _fileRepository;
        private readonly ReviewAppContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly IUserRepository _userRepository;
        public ReviewerService(IUnitOfWork unitOfWork,ICommentRepository commentRepository, IFileRepository fileRepository,IHttpContextAccessor httpContextAccessor,IPostRepository postRepository, IUserRepository userRepository,IReviewerRepository reviewerRepository,ReviewAppContext reviewAppContext)
        {
            _unitOfWork = unitOfWork;
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _accessor = httpContextAccessor;
             _reviewerRepository = reviewerRepository;
            _context = reviewAppContext;
        }

        public async Task<BaseResponse<ReviewerResponseModel>> Create(ReviewerRequestModel request)
        {
            var checkEmail = await _userRepository.GetByEmail(request.Email);
            var checkUsername = await _userRepository.Get(x => x.UserName == request.UserName);
            if (checkEmail != null)
            {
                return new BaseResponse<ReviewerResponseModel>
                {
                    Message = "Email Already Exists"
                };
            }
            if (checkUsername != null)
            {
                return new BaseResponse<ReviewerResponseModel>
                {
                    Message = "UserName Already Exists"
                };
            }
        
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

            var user = new User()
            {
                UserName = request.UserName,
                Email = request.Email,
                Password =hashedPassword,
                Salt = salt,
                Role = _context.Roles.FirstOrDefault(x => x.Name == RoleConstants.Reviewer),
                RoleId = _context.Roles.FirstOrDefault(x => x.Name == RoleConstants.Reviewer).Id

            };
            var reviewer = new Reviewer()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                ProfilePicture = await _fileRepository.UploadAsync(request.ProfilePicture),
                UserId = user.Id
            };
            await _userRepository.Create(user);
            await _reviewerRepository.Create(reviewer);
            await _unitOfWork.SaveWork();
            return new BaseResponse<ReviewerResponseModel>
            {
                Data = reviewer.Adapt<ReviewerResponseModel>(),
                Message = "Profile Created Successfully",
                Success = true
            };
        }

        public async Task<BaseResponse<ReviewerResponseModel>> DeleteReviewer(Guid id)
        {
            var reviewer = await _reviewerRepository.Get(id);
            var user = await _userRepository.Get(reviewer.UserId);
            if(reviewer == null)
            {
                return new BaseResponse<ReviewerResponseModel>
                {
                    Message = "Reviewer Not Found",
                };
            }
            reviewer.IsDeleted = true;
            user.IsDeleted = true;
             var allPost = await _postRepository.GetAll();
            var allcomment = await _commentRepository.GetAll();
            var allCommentByReviewer = allcomment.Where(x => x.ReviewerId == reviewer.Id).ToList();
             var allPostByReviewer = allPost.Where(x => x.ReviewerId == reviewer.Id).ToList();
            if(allCommentByReviewer != null )
            {
                allCommentByReviewer = allCommentByReviewer.Where(x=>x != null).ToList();
                foreach(var comment in allCommentByReviewer)
                {
                    comment.IsDeleted = true;
                }
                _commentRepository.UpdateRange(allCommentByReviewer);
            }
            if(allPostByReviewer != null)
            {
                allPostByReviewer = allPostByReviewer.Where(x => x != null).ToList();
                foreach (var post in allPostByReviewer)
                {
                    post.IsDeleted = true;
                }

                _postRepository.UpdateRange(allPostByReviewer!);
            }

            _userRepository.Update(user);
            _reviewerRepository.Update(reviewer);
            await _unitOfWork.SaveWork();
            return new BaseResponse<ReviewerResponseModel>
            {
               Message = "Deleted Successfully",
               Success = true,
            };
           
        }

        public async Task<BaseResponse<ICollection<ReviewerResponseModel>>> GetAllReviewers()
        {
            var getAllReviewers = await _reviewerRepository.GetAll();
            if(getAllReviewers.Count == 0 && getAllReviewers.All(x=>x.IsDeleted == true))
            {
                return new BaseResponse<ICollection<ReviewerResponseModel>>
                {
                    Message = "No Reviewers Found"
                };
            }
            return new BaseResponse<ICollection<ReviewerResponseModel>>
            {
                Data = getAllReviewers.SkipWhile(x => x.IsDeleted == true).Select(a => a.Adapt<ReviewerResponseModel>()).ToList(),
                Message = "All Reviewers",
                Success = true
            };
        }

        public async Task<BaseResponse<ReviewerResponseModel>> GetReviewer(Guid Id)
        {
            var reviewer = await _reviewerRepository.Get(Id);
            if (reviewer == null && reviewer.IsDeleted == true)
            {
                return new BaseResponse<ReviewerResponseModel>
                {
                    Message = "Not Found",
                };
            }
            return new BaseResponse<ReviewerResponseModel>
            {
                Data = reviewer.Adapt<ReviewerResponseModel>(),
                Message = "Found",
                Success = true,
            };

        }


        public async Task<BaseResponse<ReviewerResponseModel>> GetReviewerByLoggedInUser()
        {
            var userId = _accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
          
            Guid.TryParse(userId, out Guid userGuid);
            var user = await _userRepository.Get(userGuid);
            var getReviewer = await _reviewerRepository.Get(x => x.UserId == userGuid);
            if (getReviewer == null)
            {
                return new BaseResponse<ReviewerResponseModel>
                {
                    Message = "Not Found",
                };
            }
            var reviewer = new ReviewerResponseModel
            {
               Email = user.Email,
               DateOfBirth = getReviewer.DateOfBirth,
               LastName = getReviewer.LastName,
               FirstName = getReviewer.FirstName,
               ProfilePicture = getReviewer.ProfilePicture,
               UserName = user.UserName,
               UserId = getReviewer.UserId,
               Id = getReviewer.Id,
               
            };
            return new BaseResponse<ReviewerResponseModel>
            {
                Data = reviewer,
                Message = "Found",
                Success = true,
            };
        }
        public async Task<BaseResponse<ReviewerResponseModel>> UpdateReviewer(ReviewerUpdateModel model)
        {
            var response = await GetReviewerByLoggedInUser();
            var reviewer = await _reviewerRepository.Get(response.Data.Id);
            var user = await _userRepository.Get(response.Data.UserId);
                if (reviewer == null || user == null)
                {
                    return new BaseResponse<ReviewerResponseModel> 
                    { 
                        Success = false, 
                        Message = "Reviewer not found." 
                    };
                }
                reviewer.FirstName = model.FirstName;
                reviewer.LastName = model.LastName;
                user.UserName = model.UserName;
                user.Email = model.Email;
                reviewer.ProfilePicture = await _fileRepository.UploadAsync(model.ProfilePicture);

                 _reviewerRepository.Update(reviewer);
                 _userRepository.Update(user);
            await _unitOfWork.SaveWork();


                return new BaseResponse<ReviewerResponseModel> 
                {
                    Success = true, 
                    Data = reviewer.Adapt<ReviewerResponseModel>(),
                    Message = "Profile updated successfully." 
                };
            

        }
    }
}
