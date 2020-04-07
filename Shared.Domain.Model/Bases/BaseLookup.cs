using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Model.Bases
{

    public class BaseLookup : BaseEntity<int>, ISoftDelete
    {
        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public bool? IsConfigurable { get; set; }

        public bool? IsVisible { get; set; }

        public bool IsDeleted { get; set; }
    }
}

