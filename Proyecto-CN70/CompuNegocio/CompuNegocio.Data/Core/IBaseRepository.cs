using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Aprovi.Data.Core
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> List();
        IEnumerable<TEntity> Search(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        TEntity Find(object id);
        TEntity Add(TEntity entity);
        void Remove(TEntity entity);
        void Remove(object id);
        TEntity Update(TEntity entity);
    }
}
