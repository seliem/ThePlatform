using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Shared.CrossCutting.DomainResult;

namespace Shared.CrossCutting.RestResult
{
    public class BaseApiResponse:BaseDomainResult
    {
        public HttpStatusCode? StatusCode { get; set; } 

    }
}
