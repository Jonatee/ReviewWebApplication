using Microsoft.EntityFrameworkCore;
using Review_Web_App.Context;
using Review_Web_App.Entities;
using Review_Web_App.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Review_Web_App.Repositories.Implementations
{
    public class ReviewerRepository : BaseRepository<Reviewer>, IReviewerRepository
    {
        private readonly ReviewAppContext _context;
        public ReviewerRepository(ReviewAppContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Reviewer?> Get(Guid id)
        {
            var reviewer = await _context.Reviewers.Include(x => x.User).Include(x =>x.Bookmarks).Include(x => x.Posts).Include(x => x.Comments).Include(x => x.Likes).FirstOrDefaultAsync(x=>x.Id == id);
            return reviewer;
        }

        public async Task<Reviewer?> Get(Expression<Func<Reviewer, bool>> expression)
        {
            var reviewer = await _context.Reviewers.Include(x => x.User).Include(x =>x.Bookmarks).Include(x => x.Posts).Include(x => x.Comments).Include(x => x.Likes).FirstOrDefaultAsync(expression);
            return reviewer;
        }

        public async Task<ICollection<Reviewer?>> GetAll()
        {
            var reviewers = await _context.Reviewers.Include(x => x.User).Include(x => x.Bookmarks).Include(x=>x.Posts).Include(x=>x.Comments).Include(x=>x.Likes).ToListAsync();
            return reviewers;
        }

      

    }
}
