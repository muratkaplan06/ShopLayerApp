using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Business.Models
{
    public class PaginationModel
    {
        public List<ProductListModel> List { get; set; }
        public int SizePerPage { get; set; }
        public int TotalCount { get; set; }
    }
}
