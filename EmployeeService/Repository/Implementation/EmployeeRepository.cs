
namespace EmployeeService.Repository.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _ctx;
        public EmployeeRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<bool> AddUpdate(Employee model)
        {
            try
            {   // Add
                if (model.Id == 0)
                    await _ctx.Employees.AddAsync(model);
                //Update
                else
                    _ctx.Employees.Update(model);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var record = await GetById(id);
                if (record == null)
                    return false;
                _ctx.Employees.Remove(record);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Employee>> GetAll()
        {
            return await _ctx.Employees.ToListAsync();
        }

        public async Task<Employee> GetById(int id)
        {
            return await _ctx.Employees.FindAsync(id);
        }
    }
}
