using System;

namespace Shared.Domain.Model.Auditing
{
    public class AuditTrail
    {
        public long Id { get; set; }
        public System.Guid GuidId { get; set; }
        public string ChangeXml { get; set; }
        public System.DateTime LogDate { get; set; }
        public long UserId { get; set; }
        public Nullable<System.Guid> UserGuid { get; set; }
        public Nullable<int> ObjectTypeId { get; set; }
        public bool IsData { get; set; }
        public string EventType { get; set; }
        public string TableName { get; set; }
        public string RecordId { get; set; }
        public Nullable<System.Guid> RecordGuid { get; set; }
    }
}
