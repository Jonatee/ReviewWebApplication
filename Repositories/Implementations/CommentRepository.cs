using Microsoft.EntityFrameworkCore;
using Review_Web_App.Context;
using Review_Web_App.Entities;
using Review_Web_App.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Review_Web_App.Repositories.Implementations
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        private readonly ReviewAppContext _context;
        public CommentRepository(ReviewAppContext context) : base(context)
        {
            _context = context;
        }

       

        public async Task<Comment?> Get(Guid id)
        {
            var comment = await _context.Comments.Include(a => a.Reviewer).Include(a => a.Post).FirstOrDefaultAsync(x => x.Id == id);
            return comment;
        }

        public async  Task<Comment?> Get(Expression<Func<Comment, bool>> expression)
        {
            var comment = await _context.Comments.Include(a => a.Reviewer).Include(a => a.Post).FirstOrDefaultAsync(expression);
            return comment;
        }

        public async  Task<ICollection<Comment?>> GetAll()
        {
            var comments = await _context.Comments.Include(a => a.Reviewer).Include(a => a.Post).ToListAsync();
            return comments;
        }
        public async Task<bool> Delete(Guid Id,Guid postId, Guid reviewerId)
        {
            var comment = await _context.Comments
               .FirstOrDefaultAsync(l =>l.Id == Id && l.PostId == postId && l.ReviewerId == reviewerId);

            if (comment == null)
            {
                return false; 
            }
            _context.Comments.Remove(comment);

            return true;
        }
    }
}
