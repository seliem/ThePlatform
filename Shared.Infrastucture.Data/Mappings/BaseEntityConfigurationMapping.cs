using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamkeen.MCS.Shared.Domain.Model.Bases;

namespace Tamkeen.MCS.Shared.Infrastructure.Data.Mappings
{
    public class BaseEntityConfigurationMapping<T> : EntityTypeConfiguration<T> where T : BaseEntityWithMetaData<T>
    {
        public BaseEntityConfigurationMapping()
        {
            
            Property(p => p.CreationDate).HasColumnName("CREATION_DATE");

            ToTable(nameof(T).ToUpper())

        }
    }
}
