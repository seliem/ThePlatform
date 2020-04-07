
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Shared.Domain.Model;
using Shared.Domain.Model.Bases;

namespace Shared.Infrastucture.Data.Core.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity">entity type</typeparam>
    /// <typeparam name="K">primary key type</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {

        #region Get

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetByID(object id);

        /// <summary>
        /// Get Single Entity 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        TEntity GetSingle(Expression<Func<TEntity, bool>> expression, bool includeSoftDeleted = false,
            params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Get All  [TEntity]
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAll(bool includeSoftDeleted = false);

        /// <summary>
        /// Get All [ TEntity] Filters with Linq Expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression, bool includeSoftDeleted = false,
            params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Get All [TEntity] For Paging Idea  with Filter Expression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByProperty"></param>
        /// <param name="IsDescendingOrder"></param>
        /// <param name="rowsCount"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetAllPaged(
            Expression<Func<TEntity, bool>> expression,
            int pageNum, int pageSize,
            Func<TEntity, object> orderByProperty,
            bool IsDescendingOrder,
            out int rowsCount, bool includeSoftDeleted = false, params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Get All [TEntity] For Paging Idea Without Filter Expression
        /// </summary>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByProperty"></param>
        /// <param name="IsDescendingOrder"></param>
        /// <param name="rowsCount"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetAllPaged(
            int pageNum,
            int pageSize,
            Func<TEntity, object> orderByProperty,
            bool IsDescendingOrder,
            out int rowsCount, bool includeSoftDeleted = false, params Expression<Func<TEntity, object>>[] includeProperties);


        #endregion

        #region Count

        /// <summary>
        /// Count
        /// </summary>
        /// <returns></returns>
        long Count(bool includeSoftDeleted = false);

        /// <summary>
        /// Count with Expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        long Count(Expression<Func<TEntity, bool>> expression, bool includeSoftDeleted = false);

        #endregion

        #region Others

        /// <summary>
        /// Attach 
        /// </summary>
        /// <param name="entity"></param>
        void Attach(TEntity entity);

        /// <summary>
        /// Detach
        /// </summary>
        /// <param name="entity"></param>
        void Detach(TEntity entity);

        #endregion

        #region Add 

        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int Add(TEntity entity);

        /// <summary>
        /// BulkInsert
        /// </summary>
        /// <param name="entities">list of entities</param>
        /// <returns></returns>
        int Add(IEnumerable<TEntity> entities);
        #endregion

        #region Update

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int Update(TEntity entity);

        #endregion

        #region Delete 

        /// <summary>
        /// Delete 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int Delete(TEntity entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression">expression</param>
        /// <returns></returns>
        int Delete(Expression<Func<TEntity, bool>> expression);
        #endregion

    }
}
