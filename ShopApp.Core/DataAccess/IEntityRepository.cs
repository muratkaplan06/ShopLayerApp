using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ShopApp.Core.Entities.Abstract;
using ShopApp.Core.QueryParameters;

namespace ShopApp.Core.DataAccess
{
    public interface IEntityRepository<TEntity> where TEntity : class,IEntity,new()
    {
        TEntity Get(Expression<Func<TEntity, bool>> filter);
        List<TEntity> GetList(QueryParametersModel query,Expression<Func<TEntity, bool>> filter = null, string includes = null);
        List<TEntity> GetListNoPagination(Expression<Func<TEntity, bool>> filter = null);
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}
