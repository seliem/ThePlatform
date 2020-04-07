using System;

namespace Shared.Domain.Model.Auditing
{
    public class AuditEntity
    {
        public string ChangeXml { get; set; }
        public System.DateTime LogDate { get; set; }
        public long UserId { get; set; }
        public Nullable<int> ObjectTypeId { get; set; }
        public bool IsData { get; set; }
        public string EventType { get; set; }
        public string TableName { get; set; }
        public string RecordId { get; set; }
    }
}
