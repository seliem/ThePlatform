using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using Shared.Domain.Model.Auditing;
using Shared.Domain.Model.Bases;
using Shared.Infrastucture.Data.Core.Contracts;
using Shared.Infrastucture.Data.DBContext;

namespace Shared.Infrastucture.Data.Implementations
{

    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        /// <summary>
        /// Data Auditing Service
        /// </summary>
        public IDataAuditingService _dataAuditingService;

        /// <summary>
        /// Statuses Auditing Service
        /// </summary>
        public IStatusesAuditingService _statusesAuditingService;

        /// <summary>
        /// 
        /// </summary>
        public DbContext DbContext { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(SharedDbContext context, IDataAuditingService dataAuditingService,
            IStatusesAuditingService statusesAuditingService)
        {
            _dataAuditingService = dataAuditingService;
            _statusesAuditingService = statusesAuditingService;
            DbContext = context;
        }


        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (DbContext.Database != null && DbContext.Database.CurrentTransaction != null)
                DbContext.Database.CurrentTransaction.Dispose();

            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Commit()
        {
            try
            {
                if (DbContext.Database.CurrentTransaction == null)
                    DbContext.Database.BeginTransaction();

                var result = DbContext.SaveChanges();

                if (result != 0)
                    DbContext.Database.CurrentTransaction.Commit();

                else
                    Rollback();

                return result;

            }
            catch (DbEntityValidationException ex)
            {
                Rollback();

                string errorMessages = string.Join("; ",
               ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                   .Select(x => x.PropertyName + ": " + x.ErrorMessage));

                throw new DbEntityValidationException(errorMessages);
            }
        }

        /// <summary>
        /// Save , Audit  Changes 
        /// </summary>
        /// <returns></returns>
        public virtual int Commit(long userId)
        {
            try
            {
                if (DbContext.Database.CurrentTransaction == null)
                    DbContext.Database.BeginTransaction();

                var result = SaveChanges(userId);

                if (result != 0)
                    DbContext.Database.CurrentTransaction.Commit();

                else
                    Rollback();

                return result;

            }
            catch (DbEntityValidationException ex)
            {

                string errorMessages = string.Join("; ",
                    ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                        .Select(x => x.PropertyName + ": " + x.ErrorMessage));

                throw new DbEntityValidationException(errorMessages);
            }
        }

        #region Auditing
        ///// <summary>
        ///// Save , Audit , Event Changes 
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="dataAuditingService"></param>
        ///// <param name="statusesAuditingService"></param>
        ///// <param name="eventObject"></param>
        ///// <returns></returns>
        //public virtual int Commit(long userId, List<EventLog> eventObjects)
        //{

        //    try
        //    {
        //        if (DbContext.Database.CurrentTransaction == null)
        //            DbContext.Database.BeginTransaction();

        //        var result = SaveChanges(userId, eventObjects);

        //        if (result != 0)
        //            DbContext.Database.CurrentTransaction.Commit();

        //        else
        //            Rollback();

        //        return result;

        //    }
        //    catch (DbEntityValidationException ex)
        //    {

        //        string errorMessages = string.Join("; ",
        //            ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
        //                .Select(x => x.PropertyName + ": " + x.ErrorMessage));

        //        throw new DbEntityValidationException(errorMessages);
        //    }
        //}
        #endregion

        #region Helpers

        /// <summary>
        /// Save -Audit Changes Version 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private int SaveChanges(long userId)
        {
            //Get the changes list , but for the entity that implements Data.Entities.Auditing.IAuditable
            var entiryChanges = DbContext.ChangeTracker.Entries().Where(p => p.Entity is IAuditable).ToList();
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
                    GetAuditRecordsForChange(ent, userId, EntityState.Modified);
                }
            }
            int result = 0;

            // save the data 
            result = DbContext.SaveChanges();

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
                    GetAuditRecordsForChange(ent, userId, System.Data.Entity.EntityState.Added);
                }
            }
            //if we have added changes save them 
            if (AddedChanges.Count() != 0)
                DbContext.SaveChanges();

            return result;
        }

        ///// <summary>
        ///// Save - Audit - Event Changes Version 
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="eventLog"></param>
        ///// <returns></returns>
        //private int SaveChanges(long userId, IEnumerable<EventLog> eventLogs)
        //{

        //    //Get the changes list , but for the entity that implements Data.Entities.Auditing.IAuditable
        //    var entiryChanges = DbContext.ChangeTracker.Entries().Where(p => p.Entity is IAuditable).ToList();
        //    //Get the added changes
        //    var AddedChanges = entiryChanges.Where(p => p.State == System.Data.Entity.EntityState.Added).ToList();
        //    //Get the modified changes
        //    var ModifiedChanges = entiryChanges.Where(p => p.State == System.Data.Entity.EntityState.Modified).ToList();

        //    #region In Case Modifed Status 
        //    //for each modified changes , save the XML
        //    if (ModifiedChanges.Count() != 0)
        //    {
        //        foreach (var ent in ModifiedChanges)
        //        {
        //            // For each changed record, get the audit record entries and add them
        //            GetAuditRecordsForChange(ent, userId, System.Data.Entity.EntityState.Modified);
        //        }
        //    }
        //    int result = 0;
        //    // save the data 
        //    result = DbContext.SaveChanges();
        //    // if the changed record is zero , return 
        //    if (result <= 0)
        //        return result;

        //    #region Events 

        //    // Audit Trail New Records Added To Context .
        //    var ModifiedAuditTrailsDbEntitires = DbContext.ChangeTracker.Entries().Where(p => p.Entity is AuditTrail).ToList();
        //    if (ModifiedAuditTrailsDbEntitires.Any())
        //    {
        //        AddEventsToContext(ModifiedAuditTrailsDbEntitires, eventLogs);
        //    }

        //    #endregion

        //    #endregion

        //    #region Added Status 
        //    //add after  base.SaveChanges() to  complete object after real save ex:{PK}
        //    //for each added changes , save the XML
        //    if (AddedChanges.Count() != 0)
        //    {
        //        // Get all Added/Deleted/Modified entities (not Un modified or Detached)
        //        foreach (var ent in AddedChanges)
        //        {
        //            // For each changed record, get the audit record entries and add them
        //            GetAuditRecordsForChange(ent, userId, System.Data.Entity.EntityState.Added);
        //        }
        //    }
        //    //if we have added changes save them 
        //    if (AddedChanges.Count() != 0)
        //        DbContext.SaveChanges();

        //    #region Events 
        //    // Audit Trail New Records Added To Context .
        //    var AddingAuditTrailsDbEntitires = DbContext.ChangeTracker.Entries().Where(p => p.Entity is AuditTrail).ToList();
        //    if (AddingAuditTrailsDbEntitires.Any())
        //    {
        //        AddEventsToContext(AddingAuditTrailsDbEntitires, eventLogs);
        //    }
        //    #endregion
        //    DbContext.SaveChanges();
        //    return result;
        //    #endregion

        //}

        /// <summary>
        /// prepare Audit logs under changes types 
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="userId"></param>
        /// <param name="state"></param>
        private void GetAuditRecordsForChange(DbEntityEntry dbEntry, long userId, EntityState state)
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
                    ((SharedDbContext)DbContext).AuditTrails.Add(new AuditTrail
                    {
                        ChangeXml = item.ChangeXml,
                        EventType = item.EventType,
                        IsData = item.IsData,
                        LogDate = item.LogDate,
                        ObjectTypeId = item.ObjectTypeId,
                        RecordId = item.RecordId,
                        TableName = item.TableName,
                        UserId = item.UserId,
                        GuidId = Guid.NewGuid()

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
                        ((SharedDbContext)DbContext).AuditTrails.Add(new AuditTrail
                        {
                            ChangeXml = item.ChangeXml,
                            EventType = item.EventType,
                            IsData = item.IsData,
                            LogDate = item.LogDate,
                            ObjectTypeId = item.ObjectTypeId,
                            RecordId = item.RecordId,
                            TableName = item.TableName,
                            UserId = item.UserId,
                            GuidId = Guid.NewGuid()
                        });
                    }
                }
            }

            #endregion

        }

        ///// <summary>
        ///// Add Events To Context after update AuditTrailsId Property Value .
        ///// </summary>
        ///// <param name="entries"></param>
        ///// <param name="eventLogs"></param>
        //private void AddEventsToContext(IEnumerable<DbEntityEntry> entries, IEnumerable<EventLog> eventLogs)
        //{
        //    foreach (var eventItem in eventLogs)
        //    {
        //        foreach (var auditItem in entries)
        //        {
        //            string tableName = (auditItem.Entity as AuditTrail).TableName;
        //            var auditId = long.Parse(((auditItem.Entity as AuditTrail).Id).ToString());
        //            if (tableName == eventItem.TableName)
        //            {
        //                eventItem.AuditTrailsId = auditId;
        //                EventLog @event = new EventLog
        //                {

        //                    ItineraryKey = eventItem.ItineraryKey,
        //                    ActionId = eventItem.ActionId,
        //                    AuditTrailsId = eventItem.AuditTrailsId,
        //                    EventDate = eventItem.EventDate,
        //                    ProcessName = eventItem.ProcessName,
        //                    RoleId = eventItem.RoleId,
        //                    ServiceId = eventItem.ServiceId,
        //                    StepName = eventItem.StepName,
        //                    TableName = eventItem.TableName,
        //                    UserId = eventItem.UserId
        //                };
        //                ((SharedDbContext)DbContext).EventLogs.Add(@event);
        //            }
        //        }
        //    }
        //}

        public void Rollback()
        {
            var transaction = DbContext.Database.CurrentTransaction;

            if (transaction != null)
                transaction.Rollback();
        }
        #endregion
    }
}
