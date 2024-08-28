using Review_Web_App.Models.DTOs;

namespace Review_Web_App.Services.Interfaces
{
    public interface ICommentService
    {
        Task<BaseResponse<CommentResponseModel>> CreateComment(CommentRequestModel request);
        Task<BaseResponse<CommentResponseModel>> GetComment(CommentRequestModel request);
        Task<BaseResponse<ICollection<CommentResponseModel>>> GetAllComments();
        Task<BaseResponse<CommentResponseModel>> DeleteComments(Guid postId, Guid rewiewerId,Guid commentId);
    }
}
