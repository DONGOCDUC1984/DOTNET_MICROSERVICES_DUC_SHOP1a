namespace BusinessService.HttpClient.Interface
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetEmployees();
    }
}
