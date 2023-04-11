using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopApp.Core.Entities.Abstract;

namespace ShopApp.DataAccess.Entities
{
    public class Category:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Product> Products { get; set; }
    }
}
