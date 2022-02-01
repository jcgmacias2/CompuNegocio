using Aprovi.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Aprovi.Data.Core
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        internal CNEntities _context;
        internal DbSet<TEntity> _dbSet;

        public BaseRepository(CNEntities context)
        {
            _context = context;
            this._dbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> List()
        {
            return _dbSet.AsQueryable();
        }

        public virtual IEnumerable<TEntity> Search(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity Find(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual TEntity Add(TEntity entity)
        {
            
            _dbSet.Add(entity);
            return entity;
        }

        public virtual void Remove(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Remove(entityToDelete);
        }

        public virtual void Remove(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public virtual TEntity Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.ChangeTracker.DetectChanges();
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
