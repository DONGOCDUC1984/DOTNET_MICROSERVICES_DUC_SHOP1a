
namespace ProvinceCityService.Repository.Implementation
{
    public class ProvinceCityRepository : IProvinceCityRepository
    {
        private readonly AppDbContext _ctx;
        private readonly IKafkaProducerService _kafkaProducerService;

        public ProvinceCityRepository(AppDbContext ctx, IKafkaProducerService kafkaProducerService)
        {
            _ctx = ctx;
            _kafkaProducerService = kafkaProducerService;
        }
        public async Task<bool> AddUpdate(ProvinceCity provinceCity)
        {
            string topic = "AddUpdateProvinceCityQueue";
            string key = "AddUpdateProvinceCityKey";
            try
            {   // Add
                if (provinceCity.Id == 0)
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
                    _kafkaProducerService.SendMessage(topic, key, provinceCity);
                }
                //Update
                else
                {
                    _ctx.ProvinceCities.Update(provinceCity);
                    _kafkaProducerService.SendMessage(topic, key, provinceCity);
                }

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
            string topic = "DeleteProvinceCityQueue";
            string key = "DeleteProvinceCityKey";
            try
            {
                var record = await GetById(id);
                if (record == null)
                    return false;
                _ctx.ProvinceCities.Remove(record);
                _kafkaProducerService.SendMessage(topic, key, id);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<ProvinceCity>> GetAll()
        {
            return await _ctx.ProvinceCities.ToListAsync();
        }

        public async Task<ProvinceCity> GetById(int id)
        {
            return await _ctx.ProvinceCities.FindAsync(id);
        }
    }
}
