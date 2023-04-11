using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopApp.Core.Entities.Abstract;

namespace ShopApp.DataAccess.Entities
{
    public class BasketProduct:IEntity
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
