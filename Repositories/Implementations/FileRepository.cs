using Microsoft.Extensions.Options;
using Review_Web_App.Configurations;
using Review_Web_App.Repositories.Interfaces;

namespace Review_Web_App.Repositories.Implementations
{
    public class FileRepository : IFileRepository
    {
        private readonly FileConfiguration _configuration;
        public FileRepository(IOptions<FileConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        public async Task<string> UploadAsync(IFormFile? file)
        {
            if (file == null)
            {
                return null;
            }

            var a = file.ContentType.Split('/');
            var newName = $"IMG{a[0]}{Guid.NewGuid().ToString().Substring(6, 5)}.{a[1]}";

            var b = _configuration.Path;
            if (!Directory.Exists(b))
            {
                Directory.CreateDirectory(b);
            }

            var c = Path.Combine(b, newName);

            using (var d = new FileStream(c, FileMode.Create))
            {
                await file.CopyToAsync(d);
            }

            return newName;
        }
    }
}
