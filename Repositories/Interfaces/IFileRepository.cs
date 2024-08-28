namespace Review_Web_App.Repositories.Interfaces
{
    public interface IFileRepository
    {
        Task<string> UploadAsync(IFormFile formFile);
    }
}
