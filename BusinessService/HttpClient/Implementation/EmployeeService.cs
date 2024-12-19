using BusinessService.HttpClient.Interface;
using Newtonsoft.Json;

namespace BusinessService.HttpClient.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public EmployeeService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<List<Employee>> GetEmployees()
        {
            var client = _httpClientFactory.CreateClient("Employee");
            var response = await client.GetAsync("/api/employee/");
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<List<Employee>>(data);
            if (res != null)
            {
                return res;
            }
            return new List<Employee>();
        }
    }
}
