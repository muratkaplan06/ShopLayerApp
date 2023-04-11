using ShopApp.Core.DataAccess.EntityFramework;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.DataContext;
using ShopApp.DataAccess.Entities;

namespace ShopApp.DataAccess.Concrete.EntityFramework
{
    public class EfBasketProductDal:EfEntityRepositoryBase<BasketProduct,ShopAppDbContext>,IBasketProductDal
    {
        public EfBasketProductDal(ShopAppDbContext context):base(context)
        {
            
        }
    }
}
