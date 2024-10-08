using Sample.WebApiRestful.Data.Abstract;
using Sample.WebApiRestful.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WebApiRestful.Service
{
    public class CategoryService : ICategoryService
    {
        IRepository<Categories> _categoriesRepository;
        IDapperHelper _dapperHelper;
        public CategoryService(IRepository<Categories> categoriesRepository, IDapperHelper dapperHelper)
        {
            _categoriesRepository = categoriesRepository;
            _dapperHelper = dapperHelper;
        }

        public async Task<IEnumerable<Categories>> GetCategoryAll()
        {
            return await _categoriesRepository.GetData(null);

            //use dapper
            //string sql = $"SELECT * FROM Categories";
            //return await _dapperHelper.ExecuteSqlReturnList<Categories>(sql);
        }

        public async Task<bool> UpdateStatus(int id)
        {
            var category = await _categoriesRepository.GetById(id);
            category.IsActive = false;
            await _categoriesRepository.Commit();

            return await Task.FromResult(true); 
        }

        public string GetCategoryById(int id)
        {
            return "Candy";
        }
    }
}
