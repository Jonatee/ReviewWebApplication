namespace Review_Web_App.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveWork();
    }
}
