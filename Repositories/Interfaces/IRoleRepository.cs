using Review_Web_App.Entities;
using System.Linq.Expressions;

namespace Review_Web_App.Repositories.Interfaces
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<Role?> Get(Guid id);
        Task<Role?> Get(Expression<Func<Role, bool>> expression);
        Task<ICollection<Role?>> GetAll();
    }
}
