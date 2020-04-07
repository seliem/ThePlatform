using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Xml.Linq;
using Shared.Domain.Model.Auditing;
using Shared.Domain.Model.Extensions;
using Shared.Infrastucture.Data.Core.Contracts;

namespace Shared.Infrastucture.Data.Implementations
{
    /// <summary>
    /// contain all logic to Audit all changes on data only  
    /// </summary>
    public class DataAuditingService : IDataAuditingService
    {
        #region Added 

        /// <summary>
        /// Prepare list of Audit Logs from data properties 
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="userId"></param>
        /// <param name="changeTime"></param>
        /// <param name="tableName"></param>
        /// <param name="keyName"></param>
        /// <param name="StatusPropertyId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<AuditEntity> AddAuditFromDataProperties(DbEntityEntry dbEntry, long userId, DateTime changeTime,
            string tableName, string primaryKeyName, string isActivePropertyId, string isDeletedProperty, System.Data.Entity.EntityState state)
        {
            List<AuditEntity> auditTrailsList = new List<AuditEntity>();
            //in case add new Entiry  
            if (state == EntityState.Added)
            {
                // For Inserts, just add the whole record , the current state of the entity
                var xml = GetCurrents(dbEntry, userId, changeTime, tableName, primaryKeyName, isActivePropertyId, isDeletedProperty);
                //if no changes for some reason just return , no need to save empty xml ; a fail safe 
                if (string.IsNullOrEmpty(xml))
                    return null;
                auditTrailsList.Add(new AuditEntity()
                {
                    UserId = userId,
                    LogDate = changeTime,
                    EventType = "Add",
                    TableName = tableName,
                    RecordId = dbEntry.CurrentValues.GetValue<object>(primaryKeyName).ToString(),
                    ChangeXml = xml,
                    IsData = true
                });
            }
            return auditTrailsList;
        }

        /// <summary>
        /// Get Current Entiry for adding  
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="userId"></param>
        /// <param name="changeTime"></param>
        /// <param name="tableName"></param>
        /// <param name="keyName"></param>
        /// <param name="StatusPropertyId"></param>
        /// <returns></returns>
        public string GetCurrents(DbEntityEntry dbEntry, long userId, DateTime changeTime, string tableName,
            string primaryKeyName, string isActivePropertyId, string isDeletedProperty)
        {
            //Get the meta class
            var MetaDataClass = (MetadataTypeAttribute)(dbEntry.Entity.GetType().GetCustomAttributes(typeof(MetadataTypeAttribute), true))
                .FirstOrDefault();
            //dynamic objects 
            List<dynamic> props = new List<dynamic>();
            //if it is not null
            if (MetaDataClass != null)
            {
                props = MetaDataClass.MetadataClassType.GetProperties()
                       .Where(p => p.GetCustomAttributes(typeof(DisplayNameAttribute), false).Count() > 0)
                       .Select(p => new
                       {
                           p.Name,
                           Value = (p.GetCustomAttributes(typeof(DisplayNameAttribute), false).First()
                                 as DisplayNameAttribute).DisplayName,
                           ClassType = GetClassType(p)
                       }).ToList<dynamic>();
            }
            else
            {
                props = dbEntry.Entity.GetType().GetProperties().Select(p => new { p.Name, Value = p.Name }).ToList<dynamic>();
            }
            //Genrate the xnl using Linq 
            XElement auditXml =
                new XElement(
                              tableName,
                              new XAttribute("changeDate", changeTime),
                              new XAttribute("changedById", userId),
                              //get all  data properties not status properties 
                              from propertyName in props
                              where (propertyName.Name != primaryKeyName && (string.IsNullOrEmpty(isActivePropertyId) || propertyName.Name != isActivePropertyId) && (string.IsNullOrEmpty(isDeletedProperty) || propertyName.Name != isDeletedProperty))
                              select new XElement(propertyName.Name,
                              new XAttribute("DisplayName", propertyName.Value),
                              new XAttribute("classType", propertyName.ClassType),
                              new XElement("newValue", GetCurrentValue(dbEntry, propertyName))
                              ));
            if (auditXml.Elements().Count() == 0)
                return "";
            //add number of properties added to xml
            auditXml.Add(new XAttribute("PropertiesCount", auditXml.Elements().Count()));
            return auditXml.ToString(SaveOptions.DisableFormatting);
        }


        #endregion

        #region Modify

        /// <summary>
        /// Prepare list of Audit Logs from data properties after modify  only  
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="userId"></param>
        /// <param name="changeTime"></param>
        /// <param name="tableName"></param>
        /// <param name="keyName"></param>
        /// <param name="StatusPropertyId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<AuditEntity> ModifiedAuditFromDataProperties(DbEntityEntry dbEntry, long userId, DateTime changeTime,
                                                 string tableName, string primaryKeyName, string isActivePropertyId, string isDeletedProperty,
                                                 System.Data.Entity.EntityState state)
        {
            List<AuditEntity> auditTrailsList = new List<AuditEntity>();
            //in case Modify entiry 
            if (state == System.Data.Entity.EntityState.Modified)
            {
                // For Modified, add the changed record only , the current state of the entity , and the original
                var xml = GetModified(dbEntry, userId, changeTime, tableName, primaryKeyName, isActivePropertyId, isDeletedProperty);
                if (string.IsNullOrEmpty(xml))
                    return null;
                auditTrailsList.Add(new AuditEntity()
                {
                    UserId = userId,
                    LogDate = changeTime,
                    EventType = "Modify",
                    TableName = tableName,
                    RecordId = dbEntry.OriginalValues.GetValue<object>(primaryKeyName).ToString(),
                    ChangeXml = xml,
                    IsData = true
                });
            }
            return auditTrailsList;
        }

        /// <summary>
        /// get  modified entries 
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="userId"></param>
        /// <param name="changeTime"></param>
        /// <param name="tableName"></param>
        /// <param name="keyName"></param>
        /// <param name="StatusPropertyId"></param>
        /// <returns></returns>
        public string GetModified(DbEntityEntry dbEntry, long userId, DateTime changeTime, string tableName,
            string primaryKeyName, string isActivePropertyId, string isDeletedProperty)
        {
            var MetaDataClass = (MetadataTypeAttribute)(dbEntry.Entity.GetType().
                GetCustomAttributes(typeof(MetadataTypeAttribute), true)).FirstOrDefault();
            List<dynamic> props = new List<dynamic>();
            if (MetaDataClass != null)
                props = MetaDataClass.MetadataClassType.GetProperties()
                                                        .Where(p => p.GetCustomAttributes(typeof(DisplayNameAttribute), false).Count() > 0)
                                                        .Select(p => new
                                                        {
                                                            p.Name,
                                                            Value = (p.GetCustomAttributes(typeof(DisplayNameAttribute), false).First()
                                                            as DisplayNameAttribute).DisplayName,
                                                            ClassType = GetClassType(p)
                                                        }).ToList<dynamic>();
            else
                props = dbEntry.Entity.GetType().GetProperties().Select(p => new { p.Name, Value = p.Name }).ToList<dynamic>();
            XElement auditXml = new XElement(tableName,
                                        new XAttribute("changeDate", changeTime),
                                        new XAttribute("changedById", userId),
                                        //return only data properties changes from orignal to current & not primary key  & not status propertiy
                                        from propertyName in props
                                        where (
                                        propertyName.Name != primaryKeyName &&
                                        (string.IsNullOrEmpty(isActivePropertyId) || propertyName.Name != isActivePropertyId) &&
                                        (string.IsNullOrEmpty(isDeletedProperty) || propertyName.Name != isDeletedProperty) &&
                                        !object.Equals(dbEntry.OriginalValues.GetValue<object>(propertyName.Name),
                                        dbEntry.CurrentValues.GetValue<object>(propertyName.Name))
                                       )
                                        //select  from list properties 
                                        select new XElement(propertyName.Name,
                                                            new XAttribute("DisplayName", propertyName.Value),
                                                            new XAttribute("classType", propertyName.ClassType),
                                                            new XElement("originalValue", GetOriginalValue(dbEntry, propertyName)),
                                                            new XElement("newValue", GetCurrentValue(dbEntry, propertyName))
                                                            )
                                         );
            if (auditXml.Elements().Count() == 0)
                return "";
            auditXml.Add(new XAttribute("PropertiesCount", auditXml.Elements().Count()));
            return auditXml.ToString(SaveOptions.DisableFormatting);
        }

        /// <summary>
        /// get  original entiry before changes. 
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public dynamic GetOriginalValue(DbEntityEntry dbEntry, dynamic propertyName)
        {
            return dbEntry.OriginalValues.GetValue<object>(propertyName.Name) == null ? "N/A" :
                   dbEntry.OriginalValues.GetValue<object>(propertyName.Name).ToString();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// get current values for entiry  
        /// in Add & Modify Entiry  
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public dynamic GetCurrentValue(DbEntityEntry dbEntry, dynamic propertyName)
        {
            return dbEntry.CurrentValues.GetValue<object>(propertyName.Name) == null ? "N/A" :
                   dbEntry.CurrentValues.GetValue<object>(propertyName.Name).ToString();
        }

        /// <summary>
        /// return Business Type Metadata
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string GetClassType(System.Reflection.PropertyInfo p)
        {
            var attr = p.GetCustomAttributes(typeof(BusinessTypeAttribute), false).FirstOrDefault();

            if (attr == null || !(attr is BusinessTypeAttribute))
                return "";

            return (attr as BusinessTypeAttribute).ClassType;
        }

        #endregion

    }
}
