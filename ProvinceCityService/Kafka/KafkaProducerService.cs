
namespace ProvinceCityService.Kafka
{
    public class KafkaProducerService : IKafkaProducerService
    {
        public async void SendMessage<T>(string topic, string key, T message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                AllowAutoCreateTopics = true,
                Acks = Acks.All
            };
            using var producer = new ProducerBuilder<string, string>(config).Build();
            try
            {
                var serialized_message = JsonConvert.SerializeObject(message);
                await producer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = serialized_message });
            }
            catch (ProduceException<string, string> e)
            {

                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }

        }
    }
}
