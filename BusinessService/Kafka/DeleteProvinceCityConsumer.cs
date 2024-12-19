using Confluent.Kafka;
using Newtonsoft.Json;

namespace BusinessService.Kafka
{
    public class DeleteProvinceCityConsumer(IServiceScopeFactory scopeFactory) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            return Task.Run(() =>
            {
                _ = ConsumeAsync("DeleteProvinceCityQueue", stoppingToken);
            }, stoppingToken);
        }

        public async Task ConsumeAsync(string topic, CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(topic);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    if (consumeResult == null)
                    {
                        continue;
                    }
                    Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' at : '{consumeResult.Offset}' ");
                    var id = JsonConvert.DeserializeObject<int>(consumeResult.Message.Value);
                    using var scope = scopeFactory.CreateScope();
                    var _ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var record = await _ctx.ProvinceCities.FindAsync(id);
                    _ctx.ProvinceCities.Remove(record);
                    await _ctx.SaveChangesAsync();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            consumer.Close();
        }

    }
}
