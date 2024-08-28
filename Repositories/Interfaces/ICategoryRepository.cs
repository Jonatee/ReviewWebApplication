using Review_Web_App.Entities;
using System.Linq.Expressions;

namespace Review_Web_App.Repositories.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category?> Get(Guid id);
        Task<Category?> Get(Expression<Func<Category, bool>> expression);
        Task<ICollection<Category?>> GetAll();
        
    }
}
