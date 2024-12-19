
namespace CategoryService.Repository.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _ctx;
        private readonly IBus _bus;
        public CategoryRepository(AppDbContext ctx, IBus bus)
        {
            _ctx = ctx;
            _bus = bus;
        }
        public async Task<bool> AddUpdate(Category category)
        {
            try
            {   // Add
                if (category.Id == 0)
                {
                    // Find a new Id .Without newId, there will be an error.
                    var listId = (from x in _ctx.Categories
                                  select x.Id).ToList();
                    int newId = listId.Max() + 1;

                    // Create a new category
                    var model = new Category()
                    {
                        Id = newId,
                        Name = category.Name,
                    };
                    await _ctx.Categories.AddAsync(model);
                }
                //Update
                else
                {
                    _ctx.Categories.Update(category);
                }

                // Send by Masstransit and RabbitMq
                Uri uri = new Uri("rabbitmq://localhost/AddUpdateCategoryQueue");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(category);

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
                _ctx.Categories.Remove(record);
                var model = new CategoryDeleteDTO()
                {
                    Id = id
                };
                //Send by Masstransit and RabbitMq
                Uri uri = new Uri("rabbitmq://localhost/DeleteCategoryQueue");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(model);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Category>> GetAll()
        {
            return await _ctx.Categories.ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            return await _ctx.Categories.FindAsync(id);
        }
    }
}
