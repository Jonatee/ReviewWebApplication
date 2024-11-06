
using FuzzyString;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
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
            Random rnd = new Random();
             int num = rnd.Next(1, 6);
            if(num == 1)
            {
                var posts = await _context.Posts.Include(a => a.Reviewer).Include(x => x.Bookmarks).Include(a => a.Category).Include(a => a.Comments).Include(a => a.Likes).OrderByDescending(p => p.Likes.Count * 0.7 + p.Comments.Count * 0.3).ToListAsync();
                return posts;
            }
            else if (num == 2)
            {
                var posts = await _context.Posts.Include(a => a.Reviewer).Include(x => x.Bookmarks).Include(a => a.Category).Include(a => a.Comments).Include(a => a.Likes).OrderByDescending(p => p.Likes.Count + p.Comments.Count).ToListAsync();
                return posts;
            }
            else if(num == 3)
            {
                var posts = await _context.Posts.Include(a => a.Reviewer).Include(x => x.Bookmarks).Include(a => a.Category).Include(a => a.Comments).Include(a => a.Likes).OrderByDescending(p => p.Likes.Count - p.Comments.Count).ToListAsync();
                return posts;
            }
            else if(num == 4)
            {
                var posts = await _context.Posts.Include(a => a.Reviewer).Include(x => x.Bookmarks).Include(a => a.Category).Include(a => a.Comments).Include(a => a.Likes).OrderByDescending(p => p.DateCreated).ToListAsync();
                return posts;
            }
            else if(num == 5)
            {
                var posts = await _context.Posts.Include(a => a.Reviewer).Include(x => x.Bookmarks).Include(a => a.Category).Include(a => a.Comments).Include(a => a.Likes).OrderByDescending(p => p.Likes.Count + p.Comments.Count * 0.3).ToListAsync();
                return posts;
            }
            else
            {
                var posts = await _context.Posts.Include(a => a.Reviewer).Include(x => x.Bookmarks).Include(a => a.Category).Include(a => a.Comments).Include(a => a.Likes).OrderByDescending(p => p.Likes.Count * 0.7 + p.Comments.Count).ToListAsync();
                return posts;
            }
            
        }

        public async Task<ICollection<Post?>> SearchPosts(string title, Guid? categoryId)
        {
            var query = _context.Posts.AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrEmpty(title))
            {
                var titleWords = title.ToLower().Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

                var postInCategory = await query.ToListAsync();

                var fuzzyMatches = new List<Post?>();

                foreach (var post in postInCategory)
                {
                    if (post.PostTitle != null)
                    {
                        var postTitleWords = post.PostTitle.ToLower().Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var inputWord in titleWords)
                        {
                            foreach (var postWord in postTitleWords)
                            {
                                if (postWord.LevenshteinDistance(inputWord) <= 1)
                                {
                                    fuzzyMatches.Add(post);
                                    continue;  // Continue to the next word comparison
                                }
                            }
                        }
                    }
                }

                if (fuzzyMatches.Any())
                {
                    return fuzzyMatches;
                }

                query = query.Where(p => p.PostTitle != null && p.PostTitle.Contains(title));
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
