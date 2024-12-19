namespace BusinessService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProvinceCity> ProvinceCities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
               new Category { Id = 1, Name = "Fruit and vegetable" },
               new Category { Id = 2, Name = "Bread and cake" },
               new Category { Id = 3, Name = "Milk" }
           );
            modelBuilder.Entity<ProvinceCity>().HasData(
              new ProvinceCity { Id = 1, Name = "Ha Noi" },
              new ProvinceCity { Id = 2, Name = "Sai Gon" },
              new ProvinceCity { Id = 3, Name = "Hai Phong" }
            );
        }
    }
}
