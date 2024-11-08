using System.ComponentModel.DataAnnotations;

namespace Sample.WebApiRestful.Model
{
    public class RefreshTokenModel
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
