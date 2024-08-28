using Review_Web_App.Entities;
using System.Linq.Expressions;

namespace Review_Web_App.Repositories.Interfaces
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<Post?> Get(Guid id);
        Task<Post?> Get(Expression<Func<Post, bool>> expression);
        Task<ICollection<Post?>> GetAll();
        Task<ICollection<Post?>> SearchPosts(string title, Guid? categoryId);
        Task<IEnumerable<Post?>> GetPostsByReviewer(Guid reviewerId);

    }
}
