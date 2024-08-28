using Review_Web_App.Models.DTOs;

namespace Review_Web_App.Services.Interfaces
{
    public interface IReviewerService
    {
        Task<BaseResponse<ReviewerResponseModel>> Create(ReviewerRequestModel request);
        Task<BaseResponse<ReviewerResponseModel>> UpdateReviewer(ReviewerUpdateModel model);
        Task<BaseResponse<ReviewerResponseModel>> DeleteReviewer(Guid id);
        Task<BaseResponse<ICollection<ReviewerResponseModel>>> GetAllReviewers();
        Task<BaseResponse<ReviewerResponseModel>> GetReviewerByLoggedInUser();
        Task<BaseResponse<ReviewerResponseModel>> GetReviewer(Guid Id);

    }
}
