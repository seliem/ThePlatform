using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CrossCutting.DomainResult
{
    public class BaseDomainResult
    {
        public string BusinessStatusCode { get; set; }
        public string MessageAr { get; set; }
        public string MessageEn { get; set; }

    }
    // BaseDomainResult
}
