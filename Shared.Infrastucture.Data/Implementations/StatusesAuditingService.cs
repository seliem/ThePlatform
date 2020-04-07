using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using Shared.Infrastucture.Data.Core.Contracts;
using Shared.Domain.Model.Auditing;

namespace Shared.Infrastucture.Data.Implementations
{
    /// <summary>
    ///
    /// </summary>
    public class StatusesAuditingService : IStatusesAuditingService
    {

        #region SaveAudit

        /// <summary>
        /// Prepare Audit records after changes on status properties only not data for saving on db .
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="userId"></param>
        /// <param name="changeTime"></param>
        /// <param name="tableName"></param>
        /// <param name="keyName"></param>
        /// <param name="StatusPropertyId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<AuditEntity> SaveAuditFromStatusProperty(DbEntityEntry dbEntry, long userId, DateTime changeTime,
                                                 string tableName, string keyName, string isActivePropertyId, string isDeletedProperty,
                                                 System.Data.Entity.EntityState state)
        {
            List<AuditEntity> auditTrailsList = new List<AuditEntity>();

            #region Add

            if (state == System.Data.Entity.EntityState.Added)
            {
                // For Inserts, just add the whole record
                // If the entity implements IDescribableEntity, use the description from Describe(), otherwise use ToString()
                var xml = GetCurrentsStatus(dbEntry, userId, changeTime, tableName, isActivePropertyId, isDeletedProperty);
                if (string.IsNullOrEmpty(xml))
                    return null;
                auditTrailsList.Add(new AuditEntity()
                {
                    UserId = userId,
                    LogDate = changeTime,
                    EventType = "Add",
                    TableName = tableName,
                    RecordId = dbEntry.CurrentValues.GetValue<object>(keyName).ToString(),
                    ChangeXml = xml,
                    IsData = false
                });
            }

            #endregion

            #region Modified

            else if (state == System.Data.Entity.EntityState.Modified)
            {
                var xml = GetModifiedStatus(dbEntry, userId, changeTime, tableName, isActivePropertyId, isDeletedProperty);
                if (string.IsNullOrEmpty(xml))
                {
                    return null;
                }
                auditTrailsList.Add(new AuditEntity()
                {
                    UserId = userId,
                    LogDate = changeTime,
                    EventType = "Modified",
                    TableName = tableName,
                    RecordId = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                    ChangeXml = xml,
                    IsData = false
                });
            }

            #endregion

            #region Delete

            else if (state == System.Data.Entity.EntityState.Deleted)
            {
                var xml = GetOriginalsStatus(dbEntry, userId, changeTime, tableName, isActivePropertyId, isDeletedProperty);
                if (string.IsNullOrEmpty(xml))
                {
                    return null;
                }
                // Same with deletes, do the whole record, and use either the description from Describe() or ToString()
                auditTrailsList.Add(new AuditEntity()
                {
                    UserId = userId,
                    LogDate = changeTime,
                    EventType = "Delete",
                    TableName = tableName,
                    RecordId = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                    ChangeXml = xml,
                    IsData = false
                });
            }

            #endregion

            return auditTrailsList;
        }
        #endregion

        #region Add

        /// <summary>
        /// prepare xml contain status properties changes 
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="userId"></param>
        /// <param name="changeTime"></param>
        /// <param name="tableName"></param>
        /// <param name="isActivePropertyId"></param>
        /// <returns></returns>
        public string GetCurrentsStatus(DbEntityEntry dbEntry, long userId, DateTime changeTime, string tableName,
            string isActivePropertyId, string isDeletedProperty)
        {
            XElement auditXml = new XElement(tableName,
             new XAttribute("changeDate", changeTime),
             new XAttribute("changedById", userId),
             from propertyName in dbEntry.CurrentValues.PropertyNames
             where (propertyName == isActivePropertyId || propertyName == isDeletedProperty)
             select new XElement(propertyName, new XElement("newValue",
                        dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? "N/A" :
                        dbEntry.CurrentValues.GetValue<object>(propertyName).ToString())
                        ));
            if (auditXml.Elements().Count() == 0)
                return "";
            return auditXml.ToString(SaveOptions.DisableFormatting);
        }
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
        public string GetModifiedStatus(DbEntityEntry dbEntry, long userId, DateTime changeTime, string tableName,
            string isActivePropertyId,
            string isDeletedProperty)
        {
            XElement auditXml = new XElement(tableName,
               new XAttribute("changeDate", changeTime),
              new XAttribute("changedById", userId),
              from propertyName in dbEntry.CurrentValues.PropertyNames
              where (
              (propertyName == isActivePropertyId || propertyName == isDeletedProperty) &&
              !object.Equals(dbEntry.OriginalValues.GetValue<object>(propertyName), dbEntry.CurrentValues.GetValue<object>(propertyName))
              )
              select new XElement(propertyName,
                                  new XElement("originalValue", dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? "N/A" :
                                                                dbEntry.OriginalValues.GetValue<object>(propertyName).ToString()),
                                  new XElement("newValue", dbEntry.CurrentValues.GetValue<object>(propertyName) == null ? "N/A" :
                                                           dbEntry.CurrentValues.GetValue<object>(propertyName).ToString())
                                  )

               );
            if (auditXml.Elements().Count() == 0)
                return "";
            return auditXml.ToString(SaveOptions.DisableFormatting);
        }
        #endregion

        #region Delete 

        /// <summary>
        /// get original status 
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="userId"></param>
        /// <param name="changeTime"></param>
        /// <param name="tableName"></param>
        /// <param name="isActivePropertyId"></param>
        /// <returns></returns>
        public string GetOriginalsStatus(DbEntityEntry dbEntry, long userId, DateTime changeTime,
            string tableName, string isActivePropertyId, string isDeletedProperty)
        {
            XElement auditXml = new XElement(tableName,
                                               new XAttribute("changeDate", changeTime),
                                               new XAttribute("changedById", userId),
                                               from propertyName in dbEntry.OriginalValues.PropertyNames
                                               where (propertyName == isActivePropertyId || propertyName == isDeletedProperty)
                                               select new XElement(propertyName,
                                               new XElement("newValue", dbEntry.OriginalValues.GetValue<object>(propertyName) == null ? "N/A" :
                                               dbEntry.OriginalValues.GetValue<object>(propertyName).ToString())
                                               ));
            if (auditXml.Elements().Count() == 0)
                return "";
            return auditXml.ToString(SaveOptions.DisableFormatting);
        }

        #endregion

    }
}
