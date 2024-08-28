using Review_Web_App.Entities;
using System.Linq.Expressions;

namespace Review_Web_App.Repositories.Interfaces
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task<Comment?> Get(Guid id);
        Task<Comment?> Get(Expression<Func<Comment, bool>> expression);
        Task<ICollection<Comment?>> GetAll();
        Task<bool> Delete(Guid Id, Guid postId, Guid reviewerId);
    }
}
