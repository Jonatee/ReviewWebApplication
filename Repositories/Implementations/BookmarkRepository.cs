using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Review_Web_App.Context;
using Review_Web_App.Entities;
using Review_Web_App.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Review_Web_App.Repositories.Implementations
{
    public class BookmarkRepository : BaseRepository<Bookmark>, IBookmarkRepository
    {
        private readonly ReviewAppContext _bookmarkContext;
        public BookmarkRepository(ReviewAppContext context) : base(context)
        {
            _bookmarkContext = context;
        }

        public async Task Delete(Bookmark bookmark)
        {
            var getBookmark = await _bookmarkContext.Bookmarks.FirstOrDefaultAsync(x => x.PostId == bookmark.PostId && x.ReviewerId == bookmark.ReviewerId);
            _bookmarkContext.Bookmarks.Remove(getBookmark);
        }

        public async Task<Bookmark?> Get(Guid id)
        {
            var getBookmark = await _bookmarkContext.Bookmarks.Include(x=>x.Post).Include(x=>x.Reviewer).FirstOrDefaultAsync(x=> x.Id == id);
            return getBookmark;
        }

        public async Task<Bookmark?> Get(Expression<Func<Bookmark, bool>> expression)
        {
            var getBookmark = await _bookmarkContext.Bookmarks.Include(x => x.Post).Include(x => x.Reviewer).FirstOrDefaultAsync(expression);
            return getBookmark;
        }

        public async Task<ICollection<Bookmark?>> GetAll()
        {
            var getBookmarks = await _bookmarkContext.Bookmarks.Include(x => x.Post).Include(x => x.Reviewer).ToListAsync();
            return getBookmarks;
        }

        public async Task<IEnumerable<Post?>> GetReviewerBookmark(Guid reviewerId)
        {
            var bookmarkIds = await _bookmarkContext.Bookmarks
                                   .Where(w => w.ReviewerId == reviewerId)
                                   .Select(w => w.PostId)
                                   .ToListAsync();
            var posts = await _bookmarkContext.Posts.Where(p => bookmarkIds.Contains(p.Id)).ToListAsync();
            return posts;
        }
    }
}
