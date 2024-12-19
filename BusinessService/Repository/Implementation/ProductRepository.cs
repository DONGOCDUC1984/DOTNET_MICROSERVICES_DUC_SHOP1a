
namespace BusinessService.Repository.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _ctx;
        private readonly IInMemoryCacheService _inMemoryCacheService;
        private IEmployeeService _employeeService;
        public ProductRepository(AppDbContext ctx, IInMemoryCacheService inMemoryCacheService ,
            IEmployeeService employeeService)
        {
            _ctx = ctx;
            _inMemoryCacheService = inMemoryCacheService;
            _employeeService = employeeService;
        }

        public async Task<List<Employee>> GetEmployees()
        {
            var newEmployees = await _employeeService.GetEmployees();
            var oldEmployees = await _ctx.Employees.ToListAsync();
            _ctx.Employees.RemoveRange(oldEmployees);
            await _ctx.SaveChangesAsync();
            await _ctx.Employees.AddRangeAsync(newEmployees);
            await _ctx.SaveChangesAsync();
            return newEmployees;
        }
        public async Task<List<Category>> GetCategories()
        {
            var data = await _ctx.Categories.ToListAsync();
            return data;
        }
        public async Task<List<District>> GetDistricts()
        {
            var data = await _ctx.Districts
               .Include(x => x.ProvinceCity)
               .ToListAsync();
            return data;
        }

        public async Task<List<ProvinceCity>> GetProvinceCities()
        {
            var data = await _ctx.ProvinceCities.ToListAsync();
            return data;
        }
        public async Task<bool> AddUpdate(ProductAddUpdateDTO modelDTO)
        {
            try
            {
                var category = await _ctx.Categories.FindAsync(modelDTO.CategoryId);
                var district = await _ctx.Districts.FindAsync(modelDTO.DistrictId);
                // Add
                if (modelDTO.Id == 0)
                {
                    // Find a new Id .Without newId, there will be an error.
                    var listId = (from x in _ctx.Products
                                  select x.Id).ToList();
                    int newId = listId.Max() + 1;
                    // Create a new product
                    var product = new Product()
                    {
                        Id = newId,
                        Name = modelDTO.Name,
                        Description = modelDTO.Description,
                        Price = modelDTO.Price,
                        Category = category,
                        District = district,
                        ImageUrl = modelDTO.ImageUrl
                    };
                    await _ctx.Products.AddAsync(product);

                }
                // Update
                else
                {
                    var product = new Product()
                    {
                        Id = modelDTO.Id,
                        Name = modelDTO.Name,
                        Description = modelDTO.Description,
                        Price = modelDTO.Price,
                        Category = category,
                        District = district,
                        ImageUrl = modelDTO.ImageUrl
                    };
                    _ctx.Products.Update(product);
                }
                _inMemoryCacheService.RemoveData("Product");
                await _ctx.SaveChangesAsync();
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
                { return false; }

                _ctx.Products.Remove(record);
                _inMemoryCacheService.RemoveData("Product");
                await _ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        // Without "ref" in the following line,filteredData will not change
        private void Search(ref List<Product> filteredData, string searchStr = "", int categoryId = 0,
            int provinceCityId = 0, int districtId = 0, double minPrice = 0.0, double maxPrice = 0.0)
        {
            searchStr = searchStr?.ToLower();
            if (!string.IsNullOrEmpty(searchStr))
            {
                filteredData = filteredData.Where(x => x.Name.ToLower().Contains(searchStr)
                                               || x.Description.ToLower().Contains(searchStr)

                                               ).ToList();
            }
            if (categoryId > 0)
            {
                filteredData = filteredData.Where(x => x.Category.Id == categoryId).ToList();
            }
            if (provinceCityId > 0)
            {
                filteredData = filteredData.Where(x => x.District.ProvinceCity.Id == provinceCityId).ToList();
            }
            if (districtId > 0)
            {
                filteredData = filteredData.Where(x => x.District.Id == districtId).ToList();
            }
            if (minPrice != 0)
            {
                filteredData = filteredData.Where(x => x.Price >= minPrice).ToList();
            }
            if (maxPrice != 0)
            {
                filteredData = filteredData.Where(x => x.Price <= maxPrice).ToList();
            }
        }
        public async Task<IEnumerable<Product>> GetAll(string searchStr = "",
            int categoryId = 0, int provinceCityId = 0, int districtId = 0,
             double minPrice = 0.0, double maxPrice = 0.0)
        {
            searchStr = searchStr?.ToLower();
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            var cacheData = _inMemoryCacheService.GetData<List<Product>>("Product");
            var filteredData = cacheData;
            if (cacheData != null)
            {
                Search(ref filteredData, searchStr, categoryId, provinceCityId, districtId, minPrice, maxPrice);
                return filteredData;
            }
            var data = await _ctx.Products
                                     .Include(x => x.Category)
                                     .Include(x => x.District)
                                     .ThenInclude(x => x.ProvinceCity)
                                     .OrderBy(x => x.Id)
                                     .ToListAsync();
            _inMemoryCacheService.SetData<List<Product>>("Product", data, expirationTime);
            cacheData = _inMemoryCacheService.GetData<List<Product>>("Product");
            filteredData = cacheData;
            Search(ref filteredData, searchStr, categoryId, provinceCityId, districtId, minPrice, maxPrice);
            return filteredData;
        }

        public async Task<Product> GetById(int id)
        {
            var list = await GetAll("", 0, 0, 0, 0.0, 0.0);
            var data = list.FirstOrDefault(x => x.Id == id);
            return data;
        }
        public async Task<Product> GetOnlyProductById(int id)
        {
            // In the following line, there shoud be AsNoTracking(),Otherwise,there will be an error
            //  when we utilize this method to delete the image of the old product in Controller
            var data = await _ctx.Products.AsNoTracking()
                      .FirstOrDefaultAsync(x => x.Id == id);
            return data;
        }


    }
}
