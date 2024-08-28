using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using Review_Web_App.Constants;
using Review_Web_App.Entities;

namespace Review_Web_App.Context
{
    public class ReviewAppContext : DbContext
    {
        public ReviewAppContext(DbContextOptions<ReviewAppContext> options) : base(options){}
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Bookmark> Bookmarks => Set<Bookmark>();
        public DbSet<Reviewer> Reviewers => Set<Reviewer>();
        public DbSet<Like> Likes => Set<Like>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
  
            base.OnModelCreating(modelBuilder);
            var adminId = Guid.NewGuid();
            var reviewerId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = adminId, DateCreated = DateTime.Now, Name = RoleConstants.Admin },
                new Role { Id = reviewerId, DateCreated = DateTime.Now, Name = RoleConstants.Reviewer}
                );
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = userId,
                    Email = "AdminUser@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("AdminUser", salt),
                    DateCreated = DateTime.Now,
                    UserName = "User@",
                    Salt = salt,
                    RoleId = adminId
                }

                );
           
       
           

            modelBuilder.Entity<Reviewer>()
                .Property(r => r.DateOfBirth)
                .HasConversion<DateOnlyConverter, DateOnlyComparer>();

           
        

    }


}
}
 