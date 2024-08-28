using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Review_Web_App.Context
{
    public class DbConfig : IDesignTimeDbContextFactory<ReviewAppContext>
    {
       
            public ReviewAppContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<ReviewAppContext>();
                optionsBuilder.UseMySql("Server=localhost;Database=ReviewWebApp;User=root;Password=@Jonatee;",
                                         ServerVersion.AutoDetect("Server=localhost;Database=ReviewWebApp;User=root;Password=@Jonatee;"));

                return new ReviewAppContext(optionsBuilder.Options);
            }

    }
    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter()
            : base(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d))
        { }
    }

    public class DateOnlyComparer : ValueComparer<DateOnly>
    {
        public DateOnlyComparer()
            : base(
                (d1, d2) => d1 == d2,
                d => d.GetHashCode(),
                d => DateOnly.FromDateTime(d.ToDateTime(TimeOnly.MinValue)))
        { }
    }


}
