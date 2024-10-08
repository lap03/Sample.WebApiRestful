using System.ComponentModel.DataAnnotations;

namespace Sample.WebApiRestful.Model
{
    public class AccountModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
