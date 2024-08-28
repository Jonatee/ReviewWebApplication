using Microsoft.EntityFrameworkCore;
using Review_Web_App.Context;
using Review_Web_App.Entities;
using Review_Web_App.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Review_Web_App.Repositories.Implementations
{
    public class LikeRepository : BaseRepository<Like>, ILikeRepository
    {
        private readonly ReviewAppContext _context;
        public LikeRepository(ReviewAppContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> Delete(Guid postId, Guid reviewerId)
        {

            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.ReviewerId == reviewerId);

            if (like == null)
            {
                return false; 
            }

            _context.Likes.Remove(like);
            return true;

        }
        public async Task<Like?> Get(Guid id)
        {
            var like = await _context.Likes.Include(a => a.Post).Include(a => a.Reviewer).FirstOrDefaultAsync(x => x.Id == id);
            return like;
        }

        public async Task<Like?> Get(Expression<Func<Like, bool>> expression)
        {
            var like = await _context.Likes.Include(a => a.Post).Include(a => a.Reviewer).FirstOrDefaultAsync(expression);
            return like;
        }

        public async Task<ICollection<Like?>> GetAll()
        {
            var likes = await _context.Likes.Include(a => a.Post).Include(a => a.Reviewer).ToListAsync();
            return likes;
        }

    }
}



