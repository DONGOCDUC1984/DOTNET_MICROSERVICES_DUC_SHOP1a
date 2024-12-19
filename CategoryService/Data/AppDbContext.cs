
namespace CategoryService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Fruit and vegetable" },
                new Category { Id = 2, Name = "Bread and cake" },
                new Category { Id = 3, Name = "Milk" }
            );

        }
    }
}
