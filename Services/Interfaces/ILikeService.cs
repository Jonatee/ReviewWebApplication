using Review_Web_App.Models.DTOs;

namespace Review_Web_App.Services.Interfaces
{
    public interface ILikeService
    {
        Task<BaseResponse<LikeResponseModel>> CreateLike(LikeRequestModel request);
        Task<BaseResponse<LikeResponseModel>> RemoveLike(LikeRequestModel request);
        Task<BaseResponse<ICollection<LikeResponseModel>>> GetAllLikes();
        Task<BaseResponse<LikeResponseModel>> GetLike(Guid id);
    }
}
