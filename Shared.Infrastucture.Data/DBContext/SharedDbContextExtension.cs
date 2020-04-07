using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamkeen.MCS.Shared.Domain.Model.Auditing;
using Tamkeen.MCS.Shared.Domain.Model.Extensions;
using Tamkeen.MCS.Shared.Domain.Model.Logging;
using Tamkeen.MCS.Shared.Infrastucture.Data.Core.Contracts;

namespace Tamkeen.MCS.Shared.Infrastucture.Data.DBContext
{
    public partial class SharedDbContext
    {
        #region Db Context Methods 

        /// <summary>
        /// Save -Audit Changes Version 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int SaveChanges(long userId, IDataAuditingService _dataAuditingService, IStatusesAuditingService _statusesAuditingService)
        {
            //Get the changes list , but for the entity that implements Data.Entities.Auditing.IAuditable
            var entiryChanges = this.ChangeTracker.Entries().Where(p => p.Entity is IAuditable).ToList();
            //Get the added changes
            var AddedChanges = entiryChanges.Where(p => p.State == System.Data.Entity.EntityState.Added).ToList();
            //Get the modified changes
            var ModifiedChanges = entiryChanges.Where(p => p.State == System.Data.Entity.EntityState.Modified).ToList();
            //for each modified changes , save the XML
            if (ModifiedChanges.Count() != 0)
            {
                foreach (var ent in ModifiedChanges)
                {
                    // For each changed record, get the audit record entries and add them
                    GetAuditRecordsForChange(ent, userId, System.Data.Entity.EntityState.Modified,
                                                                  _dataAuditingService, _statusesAuditingService);
                }
            }
            int result = 0;

            // save the data 
            result = base.SaveChanges();

            // if the changed record is zero , return 
            if (result <= 0)
                return result;
            //add after  base.SaveChanges() to  complete object after real save ex:{PK}
            //for each added changes , save the XML
            if (AddedChanges.Count() != 0)
            {
                // Get all Added/Deleted/Modified entities (not Un modified or Detached)
                foreach (var ent in AddedChanges)
                {
                    // For each changed record, get the audit record entries and add them
                    GetAuditRecordsForChange(ent, userId, System.Data.Entity.EntityState.Added, _dataAuditingService, _statusesAuditingService);
                }
            }
            //if we have added changes save them 
            if (AddedChanges.Count() != 0)
                base.SaveChanges();

            return result;
        }

        /// <summary>
        /// Save - Audit - Event Changes Version 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="_dataAuditingService"></param>
        /// <param name="_statusesAuditingService"></param>
        /// <param name="eventLog"></param>
        /// <returns></returns>
        public int SaveChanges(long userId, IDataAuditingService _dataAuditingService, IStatusesAuditingService _statusesAuditingService,
            IEnumerable<EventLog> eventLogs)
        {

            //Get the changes list , but for the entity that implements Data.Entities.Auditing.IAuditable
            var entiryChanges = this.ChangeTracker.Entries().Where(p => p.Entity is IAuditable).ToList();
            //Get the added changes
            var AddedChanges = entiryChanges.Where(p => p.State == System.Data.Entity.EntityState.Added).ToList();
            //Get the modified changes
            var ModifiedChanges = entiryChanges.Where(p => p.State == System.Data.Entity.EntityState.Modified).ToList();

            #region In Case Modifed Status 
            //for each modified changes , save the XML
            if (ModifiedChanges.Count() != 0)
            {
                foreach (var ent in ModifiedChanges)
                {
                    // For each changed record, get the audit record entries and add them
                    GetAuditRecordsForChange(ent, userId, System.Data.Entity.EntityState.Modified, _dataAuditingService, _statusesAuditingService);
                }
            }
            int result = 0;
            // save the data 
            result = base.SaveChanges();
            // if the changed record is zero , return 
            if (result <= 0)
                return result;

            #region Events 

            // Audit Trail New Records Added To Context .
            var ModifiedAuditTrailsDbEntitires = this.ChangeTracker.Entries().Where(p => p.Entity is AuditTrail).ToList();
            if (ModifiedAuditTrailsDbEntitires.Any())
            {
                AddEventsToContext(ModifiedAuditTrailsDbEntitires, eventLogs);
            }

            #endregion

            #endregion

            #region Added Status 
            //add after  base.SaveChanges() to  complete object after real save ex:{PK}
            //for each added changes , save the XML
            if (AddedChanges.Count() != 0)
            {
                // Get all Added/Deleted/Modified entities (not Un modified or Detached)
                foreach (var ent in AddedChanges)
                {
                    // For each changed record, get the audit record entries and add them
                    GetAuditRecordsForChange(ent, userId, System.Data.Entity.EntityState.Added, _dataAuditingService, _statusesAuditingService);
                }
            }
            //if we have added changes save them 
            if (AddedChanges.Count() != 0)
                base.SaveChanges();

            #region Events 
            // Audit Trail New Records Added To Context .
            var AddingAuditTrailsDbEntitires = this.ChangeTracker.Entries().Where(p => p.Entity is AuditTrail).ToList();
            if (AddingAuditTrailsDbEntitires.Any())
            {
                AddEventsToContext(AddingAuditTrailsDbEntitires, eventLogs);
            }
            #endregion
            base.SaveChanges();
            return result;
            #endregion

        }

        #endregion

        #region Business Methods 

        /// <summary>
        /// prepare Audit logs under changes types 
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="userId"></param>
        /// <param name="state"></param>
        private void GetAuditRecordsForChange(DbEntityEntry dbEntry, long userId, System.Data.Entity.EntityState state
            , IDataAuditingService _dataAuditingService, IStatusesAuditingService _statusesAuditingService)
        {
            DateTime changeTime = DateTime.UtcNow;

            // Get table name by calling the GetTableName()
            string tableName = (dbEntry.Entity as IAuditable).GetTableName();
            // Get primary key value by calling the GetKeyPropertyName()
            string primaryKeyName = (dbEntry.Entity as IAuditable).GetPrimaryKeyPropertyName();
            //Get is active property  
            string isActiveProperty = (dbEntry.Entity as IAuditable).GetIsActivePropertName();
            //Get is Deleted property  
            string isDeletedProperty = (dbEntry.Entity as IAuditable).GetIsDeletedPropertName();

            #region Data Changes 

            //Save the audit from the data properties
            List<AuditEntity> auditDetails = new List<AuditEntity>();
            //in case Add only 
            if (state == System.Data.Entity.EntityState.Added)
            {
                auditDetails = _dataAuditingService.AddAuditFromDataProperties(dbEntry, userId, changeTime, tableName,
                    primaryKeyName, isActiveProperty, isDeletedProperty, state);
            }
            //In case modify only 
            else if (state == System.Data.Entity.EntityState.Modified)
            {
                auditDetails = _dataAuditingService.ModifiedAuditFromDataProperties(dbEntry, userId, changeTime, tableName,
                    primaryKeyName, isActiveProperty, isDeletedProperty, state);
            }
            //check Added & modified list 
            if (auditDetails != null && auditDetails.Any())
            {
                List<AuditTrail> auditTrail = new List<AuditTrail>();
                foreach (var item in auditDetails)
                {
                    AuditTrails.Add(new AuditTrail
                    {
                        ChangeXml = item.ChangeXml,
                        EventType = item.EventType,
                        IsData = item.IsData,
                        LogDate = item.LogDate,
                        ObjectTypeId = item.ObjectTypeId,
                        RecordID = item.RecordID,
                        TableName = item.TableName,
                        UserId = item.UserId
                    });
                }
            }

            #endregion

            #region Status Changes 

            //if the status id exists , save the audit statuses 
            if (!string.IsNullOrEmpty(isActiveProperty))
            {
                var statusesAuditTrails = _statusesAuditingService.SaveAuditFromStatusProperty(dbEntry, userId, changeTime,
                    tableName, primaryKeyName, isActiveProperty, isDeletedProperty, state);
                if (statusesAuditTrails != null && statusesAuditTrails.Any())
                {
                    List<AuditTrail> auditTrail = new List<AuditTrail>();
                    foreach (var item in statusesAuditTrails)
                    {
                        AuditTrails.Add(new AuditTrail
                        {
                            ChangeXml = item.ChangeXml,
                            EventType = item.EventType,
                            IsData = item.IsData,
                            LogDate = item.LogDate,
                            ObjectTypeId = item.ObjectTypeId,
                            RecordID = item.RecordID,
                            TableName = item.TableName,
                            UserId = item.UserId
                        });
                    }
                }
            }

            #endregion

        }

        /// <summary>
        /// Add Events To Context after update AuditTrailsId Property Value .
        /// </summary>
        /// <param name="entries"></param>
        /// <param name="eventLogs"></param>
        private void AddEventsToContext(IEnumerable<DbEntityEntry> entries, IEnumerable<EventLog> eventLogs)
        {
            foreach (var eventItem in eventLogs)
            {
                foreach (var auditItem in entries)
                {
                    string tableName = (auditItem.Entity as AuditTrail).TableName;
                    var auditId = long.Parse(((auditItem.Entity as AuditTrail).Id).ToString());
                    if (tableName == eventItem.TableName)
                    {
                        eventItem.AuditTrailsId = auditId;
                        EventLog @event = new EventLog
                        {
                            ItineraryKey = eventItem.ItineraryKey,
                            ActionId = eventItem.ActionId,
                            AuditTrailsId = eventItem.AuditTrailsId,
                            EventDate = eventItem.EventDate,
                            ProcessName = eventItem.ProcessName,
                            RoleId = eventItem.RoleId,
                            ServiceId = eventItem.ServiceId,
                            StepName = eventItem.StepName,
                            TableName = eventItem.TableName,
                            UserId = eventItem.UserId
                        };
                        EventLogs.Add(@event);
                    }
                }
            }
        }

        #endregion
    }
}
