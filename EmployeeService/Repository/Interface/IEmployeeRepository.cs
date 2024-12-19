using EmployeeService.Models;

namespace EmployeeService.Repository.Interface
{
    public interface IEmployeeRepository
    {
        Task<bool> AddUpdate(Employee model);
        Task<bool> Delete(int id);
        Task<Employee> GetById(int id);
        Task<List<Employee>> GetAll();
    }
}
