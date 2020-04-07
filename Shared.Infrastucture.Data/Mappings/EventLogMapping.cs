using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamkeen.MCS.Shared.Domain.Model.Logging;

namespace Tamkeen.MCS.Shared.Infrastucture.Data.Mappings
{

    ///// <summary>
    ///// 
    ///// </summary>
    public class EventLogMapping : EntityTypeConfiguration<EventLog>
    {
        public EventLogMapping()
        {
            //key
            HasKey(t => t.Id);

            //HasQueryFilter(m => !m.IsDeleted);

            //table
            ToTable("EventLog");

            //relation
            //HasRequired(t => t.TableRelationName).WithRequiredDependent(u => u.EventLog);
        }
    }
}
