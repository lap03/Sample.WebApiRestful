using Microsoft.AspNetCore.Mvc;
using Sample.WebApiRestful.Model;
using System.Runtime.InteropServices.Marshalling;

namespace Sample.WebApiRestful.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")] // có thể đặt mutiple route (đặt tên: lowercase, should be -)
        [Route("get-all")] // api/product/get-all
        [Route("get-lists")]
        public IActionResult GetList()
        {
            return Ok("GetList"); //200
        }

        [HttpGet("{id:int:min(10)}")] // api/product/3 -- id:int bắt buộc id trả về phải kiểu int
        public IActionResult GetById(int id)
        {
            return Ok($"Get by id: {id}");
        }

        [HttpGet("by-name")] // api/product/by-name?name=abc?sort=name?description=aaa
        public IActionResult GetByName(string name, string sort, string description)
        {
            List<string> items = null;

            if (items == null)
            {
                return NotFound(); // 404
            }

            return Ok($"Get by name: {name}");
        }

        [HttpPost]
        public IActionResult Create(ProductModel productModel)
        {
            // apicontroller có thể tự check dựa vào các DataAnnotations bên model nếu lỗi nó tự trả về badRequest
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(); // 400
            //}

            try
            {
                if (!string.IsNullOrEmpty(productModel.Name))
                {
                    return BadRequest(); // 400
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return Ok($"{productModel.Name} - {productModel.Description}");
        }

        [HttpPatch] // update 1 vài field như name...
        public IActionResult Update()
        {
            return NoContent(); // 204
        }

        [HttpPut]
        public IActionResult Modify()
        {
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteById(int id)
        {
            return Ok();
        }

        [HttpGet()]
        [Route("get-report-csv")]
        public IActionResult GetReportCSV(int id)
        {
            byte[] data = null;
            //return File(data, "text/csv");    

            var fileName = $"TestExcel.xlsx";
            var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(data, mimeType, fileName); //bold
        }
    }
}
