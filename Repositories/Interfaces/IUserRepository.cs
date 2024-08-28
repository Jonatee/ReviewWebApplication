using Review_Web_App.Entities;
using System.Linq.Expressions;

namespace Review_Web_App.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> Get(Guid id);
        Task<User?> GetByEmail(string email);
        Task<User?> Get(Expression<Func<User, bool>> expression);
        Task<ICollection<User?>> GetAll();
    }
}
