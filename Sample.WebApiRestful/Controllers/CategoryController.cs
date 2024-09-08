using Microsoft.AspNetCore.Mvc;
using Sample.WebApiRestful.Service;

namespace Sample.WebApiRestful.Controllers
{
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
        public async Task<IActionResult> Index()
        {
            return Ok(await _categoryService.GetCategories());
        }
    }
}
