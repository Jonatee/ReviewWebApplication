using Microsoft.EntityFrameworkCore;
using Review_Web_App.Context;
using Review_Web_App.Entities;
using Review_Web_App.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Review_Web_App.Repositories.Implementations
{
    public class CategoryReposiotry : BaseRepository<Category>, ICategoryRepository
    {
        private readonly ReviewAppContext _categoryContext;
        public CategoryReposiotry(ReviewAppContext context) : base(context)
        {
            _categoryContext = context;
        }


        public async Task<Category?> Get(Guid id)
        {
            var category =  await _categoryContext.Categories.Include(a => a.Posts).FirstOrDefaultAsync(x => x.Id == id);
            return category;
        }

        public  async Task<Category?> Get(Expression<Func<Category, bool>> expression)
        {
            var category = await _categoryContext.Categories.Include(a => a.Posts).FirstOrDefaultAsync(expression);
            return category;
        }

        public async Task<ICollection<Category?>> GetAll()
        {
            var categories = await _categoryContext.Categories.Include(a => a.Posts).ToListAsync();
            return categories;
        }
    }
}
