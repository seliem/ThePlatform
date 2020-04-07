using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Shared.CrossCutting.RestResult
{
    internal class HttpActionResult<T> : IHttpActionResult
    {
        private readonly ApiResult<T> _apiResult;
        private readonly HttpStatusCode _statusCode;

        public HttpActionResult(ApiResult<T> apiResult)
        {
            _statusCode = (HttpStatusCode)apiResult.StatusCode;
            _apiResult = apiResult;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(_statusCode)
            {
                Content = new ObjectContent<ApiResult<T>>(_apiResult, new JsonMediaTypeFormatter(), "application/json")
            };

            return Task.FromResult(response);
        }
    }
}
