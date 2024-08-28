using Review_Web_App.Models.DTOs;

namespace Review_Web_App.Services.Interfaces
{
    public interface IPostService
    {
        Task<BaseResponse<PostResponseModel>> CreatePost (PostRequestModel request);
        Task<BaseResponse<PostResponseModel>> DeletePost (Guid id);
        Task<BaseResponse<ICollection<PostResponseModel>>> GetAllPosts ();
        Task<BaseResponse<ICollection<PostResponseModel>>> GetReviewerPosts();
        Task<BaseResponse<ICollection<PostResponseModel>>> Search(string title, Guid? category);
        Task<BaseResponse<PostResponseModel>> GetPost(Guid Id);
    }
}
