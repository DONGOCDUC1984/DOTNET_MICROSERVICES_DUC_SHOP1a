using MassTransit;

namespace BusinessService.MasstransitRabbitMq
{
    public class CategoryAddUpdateConsumer : IConsumer<Category>
    {
        private readonly AppDbContext _ctx;
        public CategoryAddUpdateConsumer(AppDbContext ctx)
        {
            _ctx = ctx;    
        }
        public async Task Consume(ConsumeContext<Category> context)
        {
            var message = context.Message; 
            // Add
            if (message.Id == 0)
            {
                // Find a new Id .Without newId, there will be an error.
                var listId = (from x in _ctx.Categories
                              select x.Id).ToList();
                int newId = listId.Max() + 1;

                // Create a new category
                var model = new Category()
                {
                    Id = newId,
                    Name = message.Name,
                };
                await _ctx.Categories.AddAsync(model);
            }
            //Update
            else
            {
                _ctx.Categories.Update(message);
            }
                
            _ctx.SaveChanges();
        }
    }
}
