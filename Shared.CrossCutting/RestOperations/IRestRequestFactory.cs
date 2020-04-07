using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Shared.CrossCutting.RestResult;
using Shared.CrossCutting.DomainResult;

namespace Shared.CrossCutting.RestOperations
{
    public interface IRestRequestFactory
    {
        Task<ApiResult<T>> GetResourceMessageAsync<T>(string url, Dictionary<string, object> headers = null, List<string> paramters = null, Dictionary<string, string> queryStrings = null);
        Task<ApiResult<T>> PostResourceMessageAsync<T>(string url, object content, Dictionary<string, object> headers = null);
        Task<ApiResult<T>> PutResourceMessageAsync<T>(string url, object content, Dictionary<string, object> headers = null);
        Task<ApiResult<T>> DeleteResourceMessageAsync<T>(string url, Dictionary<string, object> headers = null, List<string> paramters = null, Dictionary<string, string> queryStrings = null);
        ApiResult<T> GetResourceMessage<T>(string url, Dictionary<string, object> headers = null, List<string> paramters = null, Dictionary<string, string> queryStrings = null);
        ApiResult<T> PostResourceMessage<T>(string url, object content, Dictionary<string, object> headers = null);
        ApiResult<T> PutResourceMessage<T>(string url, object content, Dictionary<string, object> headers = null);
        ApiResult<T> DeleteResourceMessage<T>(string url, Dictionary<string, object> headers = null, List<string> paramters = null, Dictionary<string, string> queryStrings = null);

    }
}
