
namespace ProvinceCityService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ProvinceCity> ProvinceCities { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProvinceCity>().HasData(
             new ProvinceCity { Id = 1, Name = "Ha Noi" },
             new ProvinceCity { Id = 2, Name = "Sai Gon" },
             new ProvinceCity { Id = 3, Name = "Hai Phong" }
           );

        }
    }
}
