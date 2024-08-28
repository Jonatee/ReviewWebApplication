using Microsoft.EntityFrameworkCore;
using Review_Web_App.Context;
using Review_Web_App.Repositories.Implementations;
using Review_Web_App.Repositories.Interfaces;
using Review_Web_App.Services.Implementations;
using Review_Web_App.Services.Interfaces;

namespace Review_Web_App.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddContext(this IServiceCollection services, string connectionString)
        {
            return services
                .AddDbContext<ReviewAppContext>(a => a.UseMySQL(connectionString));
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>))
                .AddScoped<IBookmarkRepository,BookmarkRepository>()
                .AddScoped<ICategoryRepository, CategoryReposiotry>()
                .AddScoped<ICommentRepository, CommentRepository>()
                .AddScoped<IFileRepository, FileRepository>()
                .AddScoped<ILikeRepository, LikeRepository>()
                .AddScoped<IPostRepository, PostRepository>()
                .AddScoped<IReviewerRepository,ReviewerRepository>()
                .AddScoped<IRoleRepository, RoleRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IUserRepository, UserRepository>();
                

        }
        public static IServiceCollection AddServices(this IServiceCollection services)

        {
            return services
                .AddScoped<ICategoryService, CategoryServices>()
                .AddScoped<ICommentService, CommentService>()
                .AddScoped<IEmailSender,EmailSender>()
                .AddScoped<ILikeService, LikeService>()
                .AddScoped<IBookmarkService,BookmarkService>()
                .AddScoped<IReviewerService,ReviewerService>()
                .AddScoped<IPostService, PostService>()
                .AddScoped<IUserService, UserService>();
        }
    }
}
