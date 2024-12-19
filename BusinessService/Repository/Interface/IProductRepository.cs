
namespace BusinessService.Repository.Interface
{
    public interface IProductRepository
    {
        Task<List<Employee>> GetEmployees();
        Task<List<Category>> GetCategories();
        Task<List<District>> GetDistricts();
        Task<List<ProvinceCity>> GetProvinceCities();
        Task<bool> AddUpdate(ProductAddUpdateDTO modelDTO);
        Task<bool> Delete(int id);
        Task<Product> GetById(int id);
        Task<Product> GetOnlyProductById(int id);
        Task<IEnumerable<Product>> GetAll(string searchStr = "",
            int categoryId = 0, int provinceCityId = 0, int districtId = 0,
            double minPrice = 0.0, double maxPrice = 0.0
            );
    }
}
