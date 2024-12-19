using Confluent.Kafka;
using Newtonsoft.Json;

namespace BusinessService.Kafka
{
    public class AddUpdateProvinceCityConsumer(IServiceScopeFactory scopeFactory) : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            return Task.Run(() =>
            {
                _ = ConsumeAsync("AddUpdateProvinceCityQueue", stoppingToken);
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
                    var provinceCity = JsonConvert.DeserializeObject<ProvinceCity>(consumeResult.Message.Value);
                    using var scope = scopeFactory.CreateScope();
                    var _ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    // Add
                    if (provinceCity?.Id == 0)
                    {
                        // Find a new Id .Without newId, there will be an error.
                        var listId = (from x in _ctx.ProvinceCities
                                      select x.Id).ToList();
                        int newId = listId.Max() + 1;

                        // Create a new ProvinceCity
                        var model = new ProvinceCity()
                        {
                            Id = newId,
                            Name = provinceCity.Name,
                        };
                        await _ctx.ProvinceCities.AddAsync(model);
                    }
                    //Update
                    else
                    {
                        _ctx.ProvinceCities.Update(provinceCity);
                    }

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
