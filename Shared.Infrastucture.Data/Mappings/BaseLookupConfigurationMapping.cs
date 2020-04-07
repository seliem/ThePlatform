using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamkeen.MCS.Shared.Domain.Model.Bases;

namespace Tamkeen.MCS.Transfer.Infrastructure.Data.Mappings
{
    public class BaseLookupConfigurationMapping<T> : EntityTypeConfiguration<T> where T : BaseLookup
    {

    }
}
