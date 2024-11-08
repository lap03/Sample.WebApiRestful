using Sample.WebApiRestful.Domain.Entities;

namespace Sample.WebApiRestful.Service.Abstract
{
    public interface ICategoryService
    {
        Task<IEnumerable<Categories>> GetCategoryAll();
        string GetCategoryById(int id);
        Task<bool> UpdateStatus(int id);
    }
}