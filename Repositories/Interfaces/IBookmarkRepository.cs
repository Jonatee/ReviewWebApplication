using Review_Web_App.Entities;
using System.Linq.Expressions;

namespace Review_Web_App.Repositories.Interfaces
{
    public interface IBookmarkRepository : IBaseRepository<Bookmark>
    {
        Task<Bookmark?> Get(Guid id);
        Task<Bookmark?> Get(Expression<Func<Bookmark, bool>> expression);
        Task<IEnumerable<Post?>> GetReviewerBookmark(Guid reviewerId);
        Task Delete (Bookmark bookmark);
        Task<ICollection<Bookmark?>> GetAll();
    }
}
