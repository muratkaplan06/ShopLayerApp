using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using ShopApp.Core.Entities.Abstract;
using ShopApp.Core.QueryParameters;

namespace ShopApp.Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity,TContext>:IEntityRepository<TEntity> 
        where TEntity:class,IEntity,new()
        where TContext:DbContext
    {

        protected readonly TContext _context;

        public EfEntityRepositoryBase(TContext context)
        {
            _context = context;
        }
        public TEntity? Get(Expression<Func<TEntity, bool>> filter = null) => _context.Set<TEntity>().SingleOrDefault(filter);

        public List<TEntity> GetList(QueryParametersModel queryParameters, Expression<Func<TEntity, bool>> filter = null, string includes = null)
        {
            IQueryable<TEntity> entities = filter == null ? _context.Set<TEntity>() : _context.Set<TEntity>().Where(filter);

            if (!string.IsNullOrWhiteSpace(includes))
                entities = entities.Include(includes);

           
            entities = entities.Skip(queryParameters.Size * (queryParameters.Page - 1)).Take(queryParameters.Size);


            return entities.ToList();

        }

        public List<TEntity> GetListNoPagination(Expression<Func<TEntity, bool>> filter = null)
        {
               return filter == null ? _context.Set<TEntity>().ToList() : _context.Set<TEntity>().Where(filter).ToList();

        }
        public void Add(TEntity entity)
        {
            var addedEntity = _context.Entry(entity);
            addedEntity.State = EntityState.Added;
            _context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            var removedEntity = _context.Entry(entity);
            removedEntity.State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            var updatedEntity = _context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
