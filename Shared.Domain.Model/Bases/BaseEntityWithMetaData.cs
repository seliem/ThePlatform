using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Model.Bases
{
    public class BaseEntityWithMetaData<T> : BaseEntity<T>
    {
        public DateTime CreationDate { get; set; }

        public DateTime? LastModificationDate { get; set; }

        public long ModefierUserId { get; set; }

        public long CreatorUserId { get; set; }
    }
}
