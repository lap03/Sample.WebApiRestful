using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WebApiRestful.Domain.Entities
{
    public class Products : BaseEntity
    {
        public String Name { get; set; }
        public int QuantityPerUnit { get; set; }
        public double UnitPrice { get; set; }
        public int UnitInStock { get; set; }
        public DateTime CreateDate { get; set; }
        public int UnitOnOrder { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))] // [ForeignKey("CategoryId")] tìm xem trong đây có biến nào trùg tên thì làm FR
        public Categories Categories { get; set; }


    }
}
