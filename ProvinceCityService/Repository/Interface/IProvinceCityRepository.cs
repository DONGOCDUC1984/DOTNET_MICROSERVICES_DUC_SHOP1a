
namespace ProvinceCityService.Repository.Interface
{
    public interface IProvinceCityRepository
    {
        Task<bool> AddUpdate(ProvinceCity provinceCity);
        Task<bool> Delete(int id);
        Task<ProvinceCity> GetById(int id);
        Task<List<ProvinceCity>> GetAll();
    }
}
