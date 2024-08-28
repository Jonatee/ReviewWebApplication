using Review_Web_App.Models.DTOs;

namespace Review_Web_App.Services.Interfaces
{
    public interface IBookmarkService
    {
        Task<BaseResponse<BookmarkResponseModel>> CreateBookmark(BookmarkRequestModel request);
        Task<BaseResponse<BookmarkResponseModel>> DeleteBookmark(BookmarkRequestModel request);
        Task<BaseResponse<ICollection<PostResponseModel>>> GetReviewerBookmark(Guid reviewerId);
        Task<bool> Check(Guid postId,Guid reviewerId);
        Task<BaseResponse<ICollection<BookmarkResponseModel>>> GetAllBookmarks();
    }
}
