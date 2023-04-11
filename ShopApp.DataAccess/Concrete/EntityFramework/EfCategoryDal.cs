using ShopApp.Core.DataAccess.EntityFramework;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopApp.DataAccess.DataContext;

namespace ShopApp.DataAccess.Concrete.EntityFramework
{
    public class EfCategoryDal : EfEntityRepositoryBase<Category,ShopAppDbContext>, ICategoryDal
    {
        public EfCategoryDal(ShopAppDbContext context) : base(context)
        {

        }
    }
}
