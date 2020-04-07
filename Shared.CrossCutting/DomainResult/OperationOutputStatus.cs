using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CrossCutting.DomainResult
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OperationOutputStatus
    {
        Success,
        Fail,
        ServerError,
        UnAuthorized
    }
}
