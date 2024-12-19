
namespace EmployeeService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>().HasData(
              new Employee { Id = 1, Name = "An" ,Position="Seller",YOB=1998 },
              new Employee { Id = 2, Name = "Binh", Position = "Security guard", YOB = 1999 },
              new Employee { Id = 3, Name = "Cuong", Position = "Security guard", YOB = 2000 },
              new Employee { Id = 4, Name = "Dung", Position = "Accountant", YOB = 1996 },
              new Employee { Id = 5, Name = "Duong", Position = "Cleaner", YOB = 1995 }
            );
        }
    }
}
