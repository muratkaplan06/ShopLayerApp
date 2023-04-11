using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Business.Models
{
    public class BasketProductListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }= DateTime.Now;
        public string Description { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; } = string.Empty;
        public string image3 { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
    }
}
