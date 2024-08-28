using System.Linq.Expressions;

namespace Review_Web_App.Repositories.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<bool> Check(Expression<Func<T, bool>> expression);
        Task<T> Create(T entity);
        T Update(T entity);
        void UpdateRange(IEnumerable<T> entities);

    }
}

