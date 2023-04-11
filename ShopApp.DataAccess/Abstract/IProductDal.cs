using System.Linq.Expressions;
using ShopApp.Core.DataAccess;
using ShopApp.Core.QueryParameters;
using ShopApp.DataAccess.Entities;

namespace ShopApp.DataAccess.Abstract
{
    public interface IProductDal:IEntityRepository<Product>
    {
        List<Product> GetProductWithCategoryName(QueryParametersModel queryParameters, Expression<Func<Product, bool>> filter = null);
        List<Product> GetListNoPagination(Expression<Func<Product, bool>> filter = null);
    }
}
