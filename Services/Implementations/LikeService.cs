using Mapster;
using Review_Web_App.Entities;
using Review_Web_App.Models.DTOs;
using Review_Web_App.Repositories.Interfaces;
using Review_Web_App.Services.Interfaces;

namespace Review_Web_App.Services.Implementations
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LikeService(ILikeRepository likeRepository, IUnitOfWork unitOfWork)
        {
            _likeRepository = likeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<LikeResponseModel>> CreateLike(LikeRequestModel request)
        {
            var check = await _likeRepository.Check(x => x.PostId == request.PostId && x.ReviewerId == request.ReviewerId && x.IsDeleted == false);
            if(check)
            {
                return new BaseResponse<LikeResponseModel>
                {
                    Message = "You have already Liked this post",
                    Success = false
                };
            }
            var like = new Like()
            {
                PostId = request.PostId,
                ReviewerId = request.ReviewerId
            };
            await _likeRepository.Create(like);
            await _unitOfWork.SaveWork();
            return new BaseResponse<LikeResponseModel>
            {
                Data = like.Adapt<LikeResponseModel>(),
                Message = "Liked Post",
                Success = true
            };

        }

        public async Task<BaseResponse<ICollection<LikeResponseModel>>> GetAllLikes()
        {
            var getAllLikes = await _likeRepository.GetAll();
            if(getAllLikes.All(a => a.IsDeleted == true) || getAllLikes.Count == 0)
            {
                return new BaseResponse<ICollection<LikeResponseModel>>
                {
                    Message = "No likes for this post"
                };
            }
            return new BaseResponse<ICollection<LikeResponseModel>>
            {
                Data = getAllLikes.SkipWhile(x=>x.IsDeleted == true).Select(a => a.Adapt<LikeResponseModel>()).ToList(),
                Message = "All Likes",
                Success = true
            };
        }

        public async Task<BaseResponse<LikeResponseModel>> GetLike(Guid id)
        {
            var getLike = await _likeRepository.Get(id);
            if(getLike == null || getLike.IsDeleted == true)
            {
                return new BaseResponse<LikeResponseModel>
                {
                    Message = "Not Found",

                };

            }
            return new BaseResponse<LikeResponseModel>
            {
                Data = getLike.Adapt<LikeResponseModel>(),
                Message = "Like Found",
                Success = true
            };
        }

        public async Task<BaseResponse<LikeResponseModel>> RemoveLike(LikeRequestModel request)
        {
           var like =  await _likeRepository.Delete(request.PostId,request.ReviewerId);
            if(like != true)
            {
                return new BaseResponse<LikeResponseModel>
                {
                    Message = "Like not found",
                };
            }
            await _unitOfWork.SaveWork();
            return new BaseResponse<LikeResponseModel>
            {
                Message = "Like Removed From Post",
                Success = true
            };

             
        }
    }
}
