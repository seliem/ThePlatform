using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Domain.Model.Auditing;
namespace Shared.Infrastucture.Data.Mappings
{

    /// <summary>
    /// 
    /// </summary>
    public class AuditTrailMapping : EntityTypeConfiguration<AuditTrail>
    {
        public AuditTrailMapping()
        {
            Property(p => p.EventType).HasMaxLength(200); 
            Property(p => p.TableName).HasMaxLength(200);
        }
    }
}
