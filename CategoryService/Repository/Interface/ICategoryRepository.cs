namespace CategoryService.Repository.Interface
{
    public interface ICategoryRepository
    {
        Task<bool> AddUpdate(Category category);
        Task<bool> Delete(int id);
        Task<Category> GetById(int id);
        Task<List<Category>> GetAll();
    }
}
