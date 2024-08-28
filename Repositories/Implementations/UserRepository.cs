using Microsoft.EntityFrameworkCore;
using Review_Web_App.Context;
using Review_Web_App.Entities;
using Review_Web_App.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Review_Web_App.Repositories.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ReviewAppContext _context;
        public UserRepository(ReviewAppContext context)  : base(context)
        { 
            _context = context;
        }
        public async Task<User?> Get(Guid id)
        {
            var user = await _context.Users
               .Include(u => u.Role)
               .FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted == false);
            return user;
        }

        public async Task<User?> Get(Expression<Func<User, bool>> expression)
        {
            var user = await _context.Users
              .Include(u => u.Role)
              .FirstOrDefaultAsync(expression);
            return user;
        }

        public async Task<ICollection<User?>> GetAll()
        {
            var users = await _context.Users
                 .Include(u => u.Role)
                 .ToListAsync();
            return users;
        }

        public async Task<User?> GetByEmail(string email)
        {
            var user = await _context.Users
              .Include(u => u.Role)
              .FirstOrDefaultAsync(u => String.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase) && u.IsDeleted == false);
            return user;
        }
    }
}
