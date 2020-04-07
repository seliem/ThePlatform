using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using Shared.Domain.Model;
using Shared.Domain.Model.Auditing;

namespace Shared.Infrastucture.Data.Core.Contracts
{ 
    public interface IStatusesAuditingService
    {

        #region Save Audit

        /// <summary>
        /// Save Audit Logs Form Status Properties 
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="userId"></param>
        /// <param name="changeTime"></param>
        /// <param name="tableName"></param>
        /// <param name="keyName"></param>
        /// <param name="isActivePropertyId"></param>
        /// <param name="isDeletedProperty"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        List<AuditEntity> SaveAuditFromStatusProperty(DbEntityEntry dbEntry, long userId, DateTime changeTime,
                                                string tableName, string keyName, string isActivePropertyId, string isDeletedProperty,
                                                System.Data.Entity.EntityState state);

        #endregion

        #region Add

        /// <summary>
        /// Get Current Status 
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="userId"></param>
        /// <param name="changeTime"></param>
        /// <param name="tableName"></param>
        /// <param name="isActivePropertyId"></param>
        /// <param name="isDeletedProperty"></param>
        /// <returns></returns>
        string GetCurrentsStatus(DbEntityEntry dbEntry, long userId, DateTime changeTime, string tableName,
            string isActivePropertyId, string isDeletedProperty);

        #endregion

        #region Modify

        /// <summary>
        /// Get Modified Status 
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="userId"></param>
        /// <param name="changeTime"></param>
        /// <param name="tableName"></param>
        /// <param name="isActivePropertyId"></param>
        /// <param name="isDeletedProperty"></param>
        /// <returns></returns>
        string GetModifiedStatus(DbEntityEntry dbEntry, long userId, DateTime changeTime, string tableName,
            string isActivePropertyId,
            string isDeletedProperty);

        #endregion

        #region Delete

        /// <summary>
        /// GetOriginalsStatus
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="userId"></param>
        /// <param name="changeTime"></param>
        /// <param name="tableName"></param>
        /// <param name="isActivePropertyId"></param>
        /// <param name="isDeletedProperty"></param>
        /// <returns></returns>
        string GetOriginalsStatus(DbEntityEntry dbEntry, long userId, DateTime changeTime,
            string tableName, string isActivePropertyId, string isDeletedProperty);

        #endregion
    }
}
