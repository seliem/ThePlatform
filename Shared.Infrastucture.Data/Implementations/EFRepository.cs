
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using Shared.Domain.Model;
using Shared.Domain.Model.Bases;
using Shared.Infrastucture.Data.Core.Contracts;
using Shared.Infrastucture.Data.DBContext;

namespace Shared.Infrastucture.Data.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class EFRepository<TEntity> : IDisposable, IRepository<TEntity> where TEntity : class
    {
        #region Fields and Constructors 

        /// <summary>
        /// Db Context 
        /// </summary>
        private DbContext _context;

        /// <summary>
        /// _dispose .
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="context"></param>
        public EFRepository(SharedDbContext dbContext)
        {

            _context = dbContext;

            Dispose(false);
        }

        #endregion

        #region Implemented Methods 

        #region Get

        /// <summary>
        /// Get By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetByID(object id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
                return entity;
            }
            return null;
        }


        /// <summary>
        /// Get single TEntity 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TEntity GetSingle(Expression<Func<TEntity, bool>> expression, bool includeSoftDeleted = false,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {

            IQueryable<TEntity> result = _context.Set<TEntity>().Where(expression);

            if (includeProperties.Any())
                result = includeProperties.Aggregate(result,
               (current, include) => current.Include(include));

            if (!includeSoftDeleted && typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                result = result.Where(DefaultFilter());

            return result.FirstOrDefault();

        }

        /// <summary>
        /// Return List  Of [TEntity]
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAll(bool includeSoftDeleted = false)
        {
            IQueryable<TEntity> result = _context.Set<TEntity>();

            if (!includeSoftDeleted && typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                result = result.Where(DefaultFilter());

            return result;
        }

        /// <summary>
        /// Return List  Of [TEntity] with [Expression]
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression, bool includeSoftDeleted = false,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {

            IQueryable<TEntity> result = _context.Set<TEntity>().Where(expression);

            if (includeProperties.Any())
                result = includeProperties.Aggregate(result,
               (current, include) => current.Include(include));

            if (!includeSoftDeleted && typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                result = result.Where(DefaultFilter());

            return result;
        }

        /// <summary>
        /// Get All  [TEntity] with Expression and Pagging 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByProperty"></param>
        /// <param name="IsDescendingOrder"></param>
        /// <param name="rowsCount"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetAllPaged(Expression<Func<TEntity, bool>> expression, int pageNum, int pageSize,
            Func<TEntity, object> orderByProperty, bool IsDescendingOrder, out int rowsCount,
            bool includeSoftDeleted = false, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().Where(expression);

            if (includeProperties.Any())
                query = includeProperties.Aggregate(query,
               (current, include) => current.Include(include));

            if (!includeSoftDeleted && typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                query = query.Where(DefaultFilter());

            if (pageSize <= 0)
                pageSize = 10;

            rowsCount = query.Count();
            if (rowsCount <= pageSize || pageNum <= 0)
                pageNum = 1;


            int excludedRows = (pageNum - 1) * pageSize;

            query = IsDescendingOrder ? query.OrderByDescending(orderByProperty).AsQueryable() : query.OrderBy(orderByProperty).AsQueryable();

            return query.Skip(excludedRows).Take(pageSize);

        }

        /// <summary>
        /// Get All  [TEntity] and Pagging without Expression  
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByProperty"></param>
        /// <param name="isAscendingOrder"></param>
        /// <param name="rowsCount"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetAllPaged(int pageNum, int pageSize,
           Func<TEntity, object> orderByProperty,
            bool IsDescendingOrder, out int rowsCount, bool includeSoftDeleted = false, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();

            if (includeProperties.Any())
                query = includeProperties.Aggregate(query,
               (current, include) => current.Include(include));

            if (!includeSoftDeleted && typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                query = query.Where(DefaultFilter());


            if (pageSize <= 0)
            {
                pageSize = 20;
            }
            rowsCount = query.Count();
            if (rowsCount <= pageSize || pageNum <= 0)
            {
                pageNum = 1;
            }
            int excludedRows = (pageNum - 1) * pageSize;
            query = IsDescendingOrder ? query.OrderByDescending(orderByProperty).AsQueryable() : query.OrderBy(orderByProperty).AsQueryable();

            return query.Skip(excludedRows).Take(pageSize);
        }

        #endregion

        #region Count

        /// <summary>
        /// return count of TEntity
        /// </summary>
        /// <returns></returns>
        public long Count(bool includeSoftDeleted = false)
        {

            var result = _context.Set<TEntity>().AsQueryable();

            if (!includeSoftDeleted && typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                result = result.Where(DefaultFilter());

            return result.Count();
        }

        /// <summary>
        /// return count of TEntity with Expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public long Count(Expression<Func<TEntity, bool>> expression, bool includeSoftDeleted = false)
        {

            var result = _context.Set<TEntity>().Where(expression);

            if (!includeSoftDeleted && typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                result = result.Where(DefaultFilter());

            return result.Count();

        }

        #endregion

        #region Others 

        /// <summary>
        /// Attach  TEntity
        /// </summary>
        /// <param name="entity"></param>
        public void Attach(TEntity entity)
        {
            if (_context.Database.CurrentTransaction == null)
                _context.Database.BeginTransaction();

            _context.Set<TEntity>().Attach(entity);

        }

        /// <summary>
        /// Detach TEntity
        /// </summary>
        /// <param name="entity"></param>
        public void Detach(TEntity entity)
        {
            if (_context.Database.CurrentTransaction == null)
                _context.Database.BeginTransaction();

            _context.Set<TEntity>().Remove(entity);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///   <c>true</c> to release both managed and unmanaged resources; 
        ///   <c>false</c> to release only unmanaged resources.</param>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Insert

        /// <summary>
        ///  Add Test Method 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Add(TEntity entity)
        {
            try
            {

                if (_context.Database.CurrentTransaction == null)
                    _context.Database.BeginTransaction();

                _context.Entry(entity).State = EntityState.Added;
                _context.Set<TEntity>().Add(entity);

                return 1;
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ",
                    ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                        .Select(x => x.PropertyName + ": " + x.ErrorMessage));

                throw new DbEntityValidationException(errorMessages);
            }
        }

        /// <summary>
        /// BulkInsert
        /// </summary>
        /// <param name="entities"></param>
        public virtual int Add(IEnumerable<TEntity> entities)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("entities");
                }
                if (_context.Database.CurrentTransaction == null)
                    _context.Database.BeginTransaction();

                _context.Configuration.AutoDetectChangesEnabled = false;
                _context.Set<TEntity>().AddRange(entities);

                return entities.Count();
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ",
                    ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                        .Select(x => x.PropertyName + ": " + x.ErrorMessage));

                throw new DbEntityValidationException(errorMessages);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Add Or Update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Update(TEntity entity)
        {
            try
            {
                if (_context.Database.CurrentTransaction == null)
                    _context.Database.BeginTransaction();

                var entry = _context.Entry(entity);

                //if (entry.State == EntityState.Detached)
                //{
                //    Attach(entity);
                //    _context.Entry(entity).State = EntityState.Modified;
                //}

                _context.Set<TEntity>().AddOrUpdate(entity);

                return 1;
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ",
                    ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                        .Select(x => x.PropertyName + ": " + x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
        }


        #endregion

        #region Delete

        /// <summary>
        /// Remove 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Delete(TEntity entity)
        {
            try
            {
                if (_context.Database.CurrentTransaction == null)
                    _context.Database.BeginTransaction();

                var entry = _context.Entry(entity);

                // if (entry.State == EntityState.Detached)            
                // _context.Set<TEntity>().Attach(entity);
                _context.Entry(entity).State = EntityState.Deleted;


                if (entity is ISoftDelete)
                    return SoftDelete(entity);

                _context.Set<TEntity>().Remove(entity);
                //Change Entity State 
                _context.Entry(entity).State = EntityState.Deleted;
                return 1;
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ",
                    ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                        .Select(x => x.PropertyName + ": " + x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
        }

        /// <summary>
        /// BulkDelete
        /// </summary>
        /// <param name="expression"></param>
        public virtual int Delete(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                if (_context.Database.CurrentTransaction == null)
                    _context.Database.BeginTransaction();

                IQueryable<TEntity> query = _context.Set<TEntity>().Where(expression).AsQueryable();

                foreach (TEntity obj in query)
                {
                    var entry = _context.Entry(obj);

                    //if (entry.State == EntityState.Detached)
                    // _context.Set<TEntity>().Attach(obj);
                    _context.Entry(obj).State = EntityState.Deleted;

                    if (obj is ISoftDelete)
                    {
                        return SoftDelete(obj);
                    }

                    _context.Set<TEntity>().Remove(obj);
                }

                return query.Count();
            }

            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ",
                    ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                        .Select(x => x.PropertyName + ": " + x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
        }

        #endregion

        #endregion

        #region SoftDelete
        private int SoftDelete(TEntity item)
        {
            var dbEntry = _context.Set<TEntity>().Attach(item);

            ((ISoftDelete)dbEntry).IsDeleted = true;

            return Update(item);
        }

        public static Expression<Func<TEntity, bool>> DefaultFilter()
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                return DefaultFilterSoftDelete();
            else return x => true;
        }

        public static Expression<Func<TEntity, bool>> DefaultFilterSoftDelete()
        {
            var parameterExpression = Expression.Parameter(typeof(TEntity));
            var propertyExpression = Expression.Property(parameterExpression,
                "IsDeleted");
            var notExpression = Expression.Not(propertyExpression);
            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(notExpression,
                parameterExpression);

            return lambdaExpression;
        }

        #endregion
    }
}

