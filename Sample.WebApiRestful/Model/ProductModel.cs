using System.ComponentModel.DataAnnotations;

namespace Sample.WebApiRestful.Model
{
    public class ProductModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
