using Microsoft.EntityFrameworkCore;
using Review_Web_App.Context;
using Review_Web_App.Entities;
using Review_Web_App.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Review_Web_App.Repositories.Implementations
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        private readonly ReviewAppContext _context;
        public PostRepository(ReviewAppContext context) : base(context)
        {
            _context = context;
        }

       

        public async Task<Post?> Get(Guid id)
        {
            var post = await _context.Posts.Include(a => a.Reviewer).Include(x =>x.Bookmarks).Include(a => a.Category).Include(a => a.Comments).Include(a => a.Likes).FirstOrDefaultAsync(x => x.Id == id);
            return post;
        }

        public async Task<Post?> Get(Expression<Func<Post, bool>> expression)
        {
            var post = await _context.Posts.Include(a => a.Reviewer).Include(x => x.Bookmarks).Include(a => a.Category).Include(a => a.Comments).Include(a=>a.Likes).FirstOrDefaultAsync(expression);
            return post;
        }

        public async Task<ICollection<Post?>> GetAll()
        {
            var posts = await _context.Posts.Include(a => a.Reviewer).Include(x => x.Bookmarks).Include(a => a.Category).Include(a => a.Comments).Include(a => a.Likes).ToListAsync();
            return posts;
        }

        public async Task<ICollection<Post?>> SearchPosts(string title, Guid? categoryId)
        {
           
                var query = _context.Posts.AsQueryable(); 
            //converting the entity in a way that it'll be queryable (IQueryable) 
            //it'll help the list of posts into a format that allows complex searches

                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(p => p.PostTitle.Contains(title));
                }

                if (categoryId.HasValue)
                {
                    query = query.Where(p => p.CategoryId == categoryId.Value);
                }

                return await query.ToListAsync();
            

        }
        public async Task<IEnumerable<Post>> GetPostsByReviewer(Guid reviewerId)
        {
            var posts = await _context.Posts
                                              .Where(p => p.ReviewerId == reviewerId)
                                              .ToListAsync();

            return posts;
        }
    }
}
