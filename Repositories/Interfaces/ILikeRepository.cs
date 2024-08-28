using Review_Web_App.Entities;
using System.Linq.Expressions;

namespace Review_Web_App.Repositories.Interfaces
{
    public interface ILikeRepository : IBaseRepository<Like>
    {
        Task<Like?> Get(Guid id);
        Task<Like?> Get(Expression<Func<Like, bool>> expression);
        Task<ICollection<Like?>> GetAll();
        Task<bool> Delete(Guid postId,Guid reviewerId);
       
    }
}
