using Microsoft.EntityFrameworkCore;
using Review_Web_App.Context;
using Review_Web_App.Entities;
using Review_Web_App.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Review_Web_App.Repositories.Implementations
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        private readonly ReviewAppContext _context;
        public RoleRepository(ReviewAppContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Role?> Get(Guid id)
        {
            var role = await _context.Roles 
               .Include(a => a.Users)
              .FirstOrDefaultAsync(x => x.Id == id);
            return role;
        }

        public async Task<Role?> Get(Expression<Func<Role, bool>> expression)
        {
            var role = await _context.Roles
                          .Include(a => a.Users)
                         .FirstOrDefaultAsync(expression);
            return role;
        }

        public async Task<ICollection<Role?>> GetAll()
        {
            var roles = await _context.Roles
               .Include(a => a.Users)
               .ToListAsync();
            return roles;
        }
    }
}
