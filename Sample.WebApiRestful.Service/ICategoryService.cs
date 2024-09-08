using Sample.WebApiRestful.Domain.Entities;

namespace Sample.WebApiRestful.Service
{
    public interface ICategoryService
    {
        Task<IEnumerable<Categories>> GetCategories();
    }
}