using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WebApiRestful.Domain.Entities
{
    // [Table("Account")] nếu muốn đặt tên table
    public class User : BaseEntity
    {
        [Required]
        [StringLength(150)]
        public string UserName { get; set; }
        [Required]
        [StringLength(150)]
        public string  Password{ get; set; }
        public string DisplayName { get; set; }
        public DateTime lastLoggedIn { get; set; }
        public DateTime CreatedDate { get; set;}
    }
}
