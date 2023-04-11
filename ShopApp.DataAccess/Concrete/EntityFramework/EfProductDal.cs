using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopApp.Core.DataAccess.EntityFramework;
using ShopApp.Core.Entities.Abstract;
using ShopApp.Core.QueryParameters;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.DataContext;
using ShopApp.DataAccess.Entities;

namespace ShopApp.DataAccess.Concrete.EntityFramework
{
    public class EfProductDal:EfEntityRepositoryBase<Product,ShopAppDbContext>,IProductDal
    {
        public EfProductDal(ShopAppDbContext context) : base(context)
        {

        }

        public List<Product> GetProductWithCategoryName(QueryParametersModel queryParameters,Expression<Func<Product, bool>> filter = null)
        {
            IQueryable<Product> query = (filter == null) ? base._context.Products
                : base._context.Products.Where(filter);

            var pageQuery = query.Skip(queryParameters.Size * (queryParameters.Page - 1)).Take(queryParameters.Size);


            return pageQuery.Include(x => x.Category).ToList();
            
        }
        public List<Product> GetListNoPagination(Expression<Func<Product, bool>> filter = null)
        {
            IQueryable<Product> query = (filter == null) ? base._context.Products
                : base._context.Products.Where(filter);


            return query.Include(x => x.Category).ToList();
        }
    }

    
}
