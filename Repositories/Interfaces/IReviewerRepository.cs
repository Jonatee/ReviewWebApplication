using Review_Web_App.Entities;
using System.Linq.Expressions;

namespace Review_Web_App.Repositories.Interfaces
{
    public interface IReviewerRepository : IBaseRepository<Reviewer>
    {
        Task<Reviewer?> Get(Guid id);
        Task<Reviewer?> Get(Expression<Func<Reviewer, bool>> expression);
        Task<ICollection<Reviewer?>> GetAll();
        
       
        

    }
}
