using MassTransit;

namespace BusinessService.MasstransitRabbitMq
{
    public class CategoryDeleteConsumer : IConsumer<CategoryDeleteDTO>
    {
        private readonly AppDbContext _ctx;
        public CategoryDeleteConsumer(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task Consume(ConsumeContext<CategoryDeleteDTO> context)
        {
            var message = context.Message;
            var record = await _ctx.Categories.FindAsync(message.Id);
            _ctx.Categories.Remove(record);
            _ctx.SaveChanges();
        }
    }
}
