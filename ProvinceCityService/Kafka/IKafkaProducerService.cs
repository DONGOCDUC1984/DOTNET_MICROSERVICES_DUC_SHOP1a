namespace ProvinceCityService.Kafka
{
    public interface IKafkaProducerService
    {
        public void SendMessage<T>(string topic, string key, T message);
    }
}
