using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using Shared.Domain.Model.Auditing;

namespace Shared.Infrastucture.Data.Core.Contracts
{
    public interface IDataAuditingService
    {
        #region Added

        /// <summary>
        /// Add Audit after changes on data .
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="userId"></param>
        /// <param name="changeTime"></param>
        /// <param name="tableName"></param>
        /// <param name="primaryKeyName"></param>
        /// <param name="isActivePropertyId"></param>
        /// <param name="isDeletedProperty"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        List<AuditEntity> AddAuditFromDataProperties(DbEntityEntry dbEntry, long userId, DateTime changeTime,
            string tableName, string primaryKeyName, string isActivePropertyId, string isDeletedProperty, System.Data.Entity.EntityState state);

        /// <summary>
        /// Get Currents 
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="userId"></param>
        /// <param name="changeTime"></param>
        /// <param name="tableName"></param>
        /// <param name="primaryKeyName"></param>
        /// <param name="isActivePropertyId"></param>
        /// <param name="isDeletedProperty"></param>
        /// <returns></returns>
        string GetCurrents(DbEntityEntry dbEntry, long userId, DateTime changeTime, string tableName,
            string primaryKeyName, string isActivePropertyId, string isDeletedProperty);

        #endregion

        #region Modify

        /// <summary>
        /// Modified Audit From Data Properties
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="userId"></param>
        /// <param name="changeTime"></param>
        /// <param name="tableName"></param>
        /// <param name="primaryKeyName"></param>
        /// <param name="isActivePropertyId"></param>
        /// <param name="isDeletedProperty"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        List<AuditEntity> ModifiedAuditFromDataProperties(DbEntityEntry dbEntry, long userId, DateTime changeTime,
                                                 string tableName, string primaryKeyName, string isActivePropertyId, string isDeletedProperty,
                                                 System.Data.Entity.EntityState state);
        /// <summary>
        /// Get Modified 
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="userId"></param>
        /// <param name="changeTime"></param>
        /// <param name="tableName"></param>
        /// <param name="primaryKeyName"></param>
        /// <param name="isActivePropertyId"></param>
        /// <param name="isDeletedProperty"></param>
        /// <returns></returns>
        string GetModified(DbEntityEntry dbEntry, long userId, DateTime changeTime, string tableName,
            string primaryKeyName, string isActivePropertyId, string isDeletedProperty);

        /// <summary>
        /// Get Original Values .
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        dynamic GetOriginalValue(DbEntityEntry dbEntry, dynamic propertyName);

        #endregion

        #region Helper Methods

        /// <summary>
        /// Get Current Values .
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        dynamic GetCurrentValue(DbEntityEntry dbEntry, dynamic propertyName);

        /// <summary>
        /// Get Class Type ;
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        string GetClassType(System.Reflection.PropertyInfo p);

        #endregion
    }
}
