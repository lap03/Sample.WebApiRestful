using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.WebApiRestful.Service;

namespace Sample.WebApiRestful.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllCategory()
        {
            return Ok(await _categoryService.GetCategoryAll());
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateStatusAsynce(int id)
        {
            return Ok(await _categoryService.UpdateStatus(id));
        }


        // gọi 1 phương thức bất đồng bộ
        [HttpGet("get-name-category-by-id")]
        public async Task<IActionResult> GetCategoryNameByIdAsync(int id)
        {
            IActionResult actionResult = null;
            await Task.Factory.StartNew(() =>
            {
                actionResult = Ok(_categoryService.GetCategoryById(id));
            });

            return actionResult;
        }
    }
}
