using Microsoft.EntityFrameworkCore;
using Review_Web_App.Context;
using Review_Web_App.Repositories.Interfaces;

namespace Review_Web_App.Repositories.Implementations
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        protected ReviewAppContext _baseContext;
        public BaseRepository(ReviewAppContext context)
        {
            _baseContext = context;
        }
       
        public async Task<bool> Check(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            var exist = await _baseContext.Set<T>().AnyAsync(expression);
            return exist;
        }

        public async Task<T> Create(T entity)
        {
            await _baseContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public T Update(T entity)
        {
            _baseContext.Set<T>().Update(entity);
            return entity;
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _baseContext.Entry(entity).State = EntityState.Modified;
            }
        }
    }
}
