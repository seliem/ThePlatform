using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using Shared.CrossCutting.RestResult;
using Shared.CrossCutting.DomainResult;
using Shared.CrossCutting.Logging;

namespace Shared.CrossCutting.RestOperations
{
    public class RestRequestFactory : IRestRequestFactory
    {
        private readonly IFileLogger _logger;

        public RestRequestFactory(IFileLogger logger)
        {
            _logger = logger;

        }
        public async Task<ApiResult<T>> GetResourceMessageAsync<T>(string url, Dictionary<string, object> headers = null,
            List<string> paramters = null, Dictionary<string, string> queryStrings = null)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

                using (var client = new HttpClient())
                {
                    //Passing service base url  
                    client.BaseAddress = new Uri(url);
                    AppendHeaders(headers, client);

                    var fullUrl = AppendQueryValues(url, paramters, queryStrings);
                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  

                    var response = await client.GetAsync(fullUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        //ReadAsAsync permits to deserialize the response content
                        var responseContent = await response.Content.ReadAsAsync<T>();

                        return ConstructSuccessMessage(response, responseContent);
                    }
                    else
                    {
                        var message = await response.Content.ReadAsStringAsync();

                        return ConstructFailureMessage<T>(response, message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Error: {nameof(GetResourceMessageAsync)}", $"{nameof(GetResourceMessageAsync)} Throws Exception Url:{url}", ex, true);
                _logger.LogToElmah(ex);

                return new ApiResult<T>()
                {
                    BusinessStatusCode = HttpStatusCode.InternalServerError.ToString(),
                    Status = OperationOutputStatus.ServerError,
                    StatusCode = HttpStatusCode.InternalServerError,
                    MessageAr = $"{ExceptionMessage.GetErrorMessage(ex, "ar")}  {ex.Message}",
                    MessageEn = $"{ExceptionMessage.GetErrorMessage(ex, "en")}  {ex.Message}"
                };
            }
        }

        public async Task<ApiResult<T>> PostResourceMessageAsync<T>(string url, object content,
            Dictionary<string, object> headers = null)
        {

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

                using (var client = new HttpClient())
                {
                    //Base Address
                    client.BaseAddress = new Uri(url);
                    AppendHeaders(headers, client);

                    //Model will be serialized automatically.
                    var response = await client.PostAsJsonAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        //ReadAsAsync permits to deserialize the response content
                        var responseContent = await response.Content.ReadAsAsync<T>();

                        return ConstructSuccessMessage(response, responseContent);
                    }
                    else
                    {
                        var message = await response.Content.ReadAsStringAsync();

                        return ConstructFailureMessage<T>(response, message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Error: {nameof(PostResourceMessageAsync)}", $"{nameof(PostResourceMessageAsync)} Throws Exception Url:{url}", ex, true);
                _logger.LogToElmah(ex);

                return new ApiResult<T>()
                {
                    BusinessStatusCode = HttpStatusCode.InternalServerError.ToString(),
                    Status = OperationOutputStatus.ServerError,
                    StatusCode = HttpStatusCode.InternalServerError,
                    MessageAr = $"{ExceptionMessage.GetErrorMessage(ex, "ar")}  {ex.Message}",
                    MessageEn = $"{ExceptionMessage.GetErrorMessage(ex, "en")}  {ex.Message}"
                };
            }
        }

        public async Task<ApiResult<T>> PutResourceMessageAsync<T>(string url, object content,
            Dictionary<string, object> headers = null)
        {

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);

                    AppendHeaders(headers, client);

                    //Convert To Bytes Array .
                    var myContent = JsonConvert.SerializeObject(content);
                    var buffer = Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    //Model will be serialized automatically.
                    var response = await client.PutAsJsonAsync(url, byteContent);

                    if (response.IsSuccessStatusCode)
                    {
                        //ReadAsAsync permits to deserialize the response content
                        var responseContent = await response.Content.ReadAsAsync<T>();

                        return ConstructSuccessMessage(response, responseContent);

                    }

                    var message = await response.Content.ReadAsStringAsync();

                    return ConstructFailureMessage<T>(response, message);
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Error: {nameof(PutResourceMessageAsync)}", $"{nameof(PutResourceMessageAsync)} Throws Exception Url:{url}", ex, true);
                _logger.LogToElmah(ex);

                return new ApiResult<T>()
                {
                    BusinessStatusCode = HttpStatusCode.InternalServerError.ToString(),
                    Status = OperationOutputStatus.ServerError,
                    StatusCode = HttpStatusCode.InternalServerError,
                    MessageAr = $"{ExceptionMessage.GetErrorMessage(ex, "ar")}  {ex.Message}",
                    MessageEn = $"{ExceptionMessage.GetErrorMessage(ex, "en")}  {ex.Message}"
                };
            }
        }

        public async Task<ApiResult<T>> DeleteResourceMessageAsync<T>(string url,
            Dictionary<string, object> headers = null, List<string> paramters = null, Dictionary<string, string> queryStrings = null)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);

                    AppendHeaders(headers, client);

                    var fullUrl = AppendQueryValues(url, paramters, queryStrings);

                    HttpResponseMessage response = await client.DeleteAsync(fullUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        //ReadAsAsync permits to deserialize the response content
                        var responseContent = await response.Content.ReadAsAsync<T>();

                        return ConstructSuccessMessage(response, responseContent);
                    }

                    var message = await response.Content.ReadAsStringAsync();

                    return ConstructFailureMessage<T>(response, message);
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Error: {nameof(DeleteResourceMessageAsync)}", $"{nameof(DeleteResourceMessageAsync)} Throws Exception Url:{url}", ex, true);
                _logger.LogToElmah(ex);

                return new ApiResult<T>()
                {
                    BusinessStatusCode = HttpStatusCode.InternalServerError.ToString(),
                    Status = OperationOutputStatus.ServerError,
                    StatusCode = HttpStatusCode.InternalServerError,
                    MessageAr = $"{ExceptionMessage.GetErrorMessage(ex, "ar")}  {ex.Message}",
                    MessageEn = $"{ExceptionMessage.GetErrorMessage(ex, "en")}  {ex.Message}"
                };
            }
        }

        public ApiResult<T> GetResourceMessage<T>(string url,
            Dictionary<string, object> headers = null, List<string> paramters = null, Dictionary<string, string> queryStrings = null)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

                using (var client = new HttpClient())
                {
                    //Passing service base url  
                    client.BaseAddress = new Uri(url);
                    AppendHeaders(headers, client);


                    var fullUrl = AppendQueryValues(url, paramters, queryStrings);

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    var response = client.GetAsync(fullUrl).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        //ReadAsAsync permits to deserialize the response content
                        var responseContent = response.Content.ReadAsAsync<T>().GetAwaiter().GetResult();

                        return ConstructSuccessMessage(response, responseContent);

                    }
                    else
                    {
                        var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        return ConstructFailureMessage<T>(response, message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Error: {nameof(GetResourceMessage)}", $"{nameof(GetResourceMessage)} Throws Exception Url:{url}", ex, true);
                _logger.LogToElmah(ex);

                return new ApiResult<T>()
                {
                    BusinessStatusCode = HttpStatusCode.InternalServerError.ToString(),
                    Status = OperationOutputStatus.ServerError,
                    StatusCode = HttpStatusCode.InternalServerError,
                    MessageAr = $"{ExceptionMessage.GetErrorMessage(ex, "ar")}  {ex.Message}",
                    MessageEn = $"{ExceptionMessage.GetErrorMessage(ex, "en")}  {ex.Message}",

                };
            }
        }

        public ApiResult<T> PostResourceMessage<T>(string url, object content,
            Dictionary<string, object> headers = null)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

                using (var client = new HttpClient())
                {
                    //Base Address
                    client.BaseAddress = new Uri(url);
                    AppendHeaders(headers, client);

                    //Model will be serialized automatically.
                    var response = client.PostAsJsonAsync(url, content).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        //ReadAsAsync permits to deserialize the response content
                        var responseContent = response.Content.ReadAsAsync<T>().GetAwaiter().GetResult();

                        return ConstructSuccessMessage(response, responseContent);

                    }
                    else
                    {
                        var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        return ConstructFailureMessage<T>(response, message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Error: {nameof(PostResourceMessage)}", $"{nameof(PostResourceMessage)} Throws Exception Url:{url}", ex, true);
                _logger.LogToElmah(ex);

                return new ApiResult<T>()
                {
                    BusinessStatusCode = HttpStatusCode.InternalServerError.ToString(),
                    Status = OperationOutputStatus.ServerError,
                    StatusCode = HttpStatusCode.InternalServerError,
                    MessageAr = $"{ExceptionMessage.GetErrorMessage(ex, "ar")}  {ex.Message}",
                    MessageEn = $"{ExceptionMessage.GetErrorMessage(ex, "en")}  {ex.Message}",

                };
            }
        }

        public ApiResult<T> PutResourceMessage<T>(string url, object content,
            Dictionary<string, object> headers = null)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);

                    AppendHeaders(headers, client);

                    //Convert To Bytes Array .
                    var myContent = JsonConvert.SerializeObject(content);
                    var buffer = Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    //Model will be serialized automatically.
                    var response = client.PutAsJsonAsync(url, byteContent).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        //ReadAsAsync permits to deserialize the response content
                        var responseContent = response.Content.ReadAsAsync<T>().GetAwaiter().GetResult();

                        return ConstructSuccessMessage(response, responseContent);

                    }

                    var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    return ConstructFailureMessage<T>(response, message);
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Error: {nameof(PutResourceMessage)}", $"{nameof(PutResourceMessage)} Throws Exception Url:{url}", ex, true);
                _logger.LogToElmah(ex);

                return new ApiResult<T>()
                {
                    BusinessStatusCode = HttpStatusCode.InternalServerError.ToString(),
                    Status = OperationOutputStatus.ServerError,
                    StatusCode = HttpStatusCode.InternalServerError,
                    MessageAr = $"{ExceptionMessage.GetErrorMessage(ex, "ar")}  {ex.Message}",
                    MessageEn = $"{ExceptionMessage.GetErrorMessage(ex, "en")}  {ex.Message}",

                };
            }
        }

        public ApiResult<T> DeleteResourceMessage<T>(string url,
            Dictionary<string, object> headers = null, List<string> paramters = null, Dictionary<string, string> queryStrings = null)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);

                    AppendHeaders(headers, client);

                    var fullUrl = AppendQueryValues(url, paramters, queryStrings);

                    HttpResponseMessage response = client.DeleteAsync(fullUrl).GetAwaiter().GetResult();

                    if (response.IsSuccessStatusCode)
                    {
                        //ReadAsAsync permits to deserialize the response content
                        var responseContent = response.Content.ReadAsAsync<T>().GetAwaiter().GetResult();

                        return ConstructSuccessMessage(response, responseContent);
                    }

                    var message = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    return ConstructFailureMessage<T>(response, message);
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Error: {nameof(DeleteResourceMessage)}", $"{nameof(DeleteResourceMessage)} Throws Exception Url:{url}", ex, true);
                _logger.LogToElmah(ex);
                return new ApiResult<T>()
                {
                    BusinessStatusCode = HttpStatusCode.InternalServerError.ToString(),
                    Status = OperationOutputStatus.ServerError,
                    StatusCode = HttpStatusCode.InternalServerError,
                    MessageAr = $"{ExceptionMessage.GetErrorMessage(ex, "ar")}  {ex.Message}",
                    MessageEn = $"{ExceptionMessage.GetErrorMessage(ex, "en")}  {ex.Message}",

                };
            }
        }

        private static string AppendQueryValues(string url, List<string> paramters, Dictionary<string, string> queryStrings)
        {
            if (paramters != null)
            {
                var urlParams = new StringBuilder();
                foreach (string param in paramters)
                {
                    urlParams.Append(param + "/");
                }
                var paramList = urlParams.ToString();

                url = $"{url}/{paramList}";
            }

            if (queryStrings != null)
            {
                if (!url.Contains("?"))
                    url += "?";
                foreach (var qStr in queryStrings)
                {

                    url += $"&{qStr.Key}={qStr.Value}";
                }
            }
            return url;
        }

        private ApiResult<T> ConstructFailureMessage<T>(HttpResponseMessage response, string message)
        {
            var result = new ApiResult<T>()
            {
                StatusCode = response.StatusCode,

                MessageAr = message,
                MessageEn = message,

            };

            string request = response.RequestMessage.Content != null ? JsonConvert.SerializeObject(response?.RequestMessage?.Content) : "";


            if (((int)response.StatusCode >= 400) && ((int)response.StatusCode <= 499))
            {
                result.Status = OperationOutputStatus.Fail;
                result.BusinessStatusCode = HttpStatusCode.BadRequest.ToString();
                _logger.Log($"Failure: {response.StatusCode}", $"Call API Failure: URL: {response?.RequestMessage?.RequestUri}, Request: {request}, Message: {message}, StatusCode: {response.StatusCode}", true);

            }

            else if (((int)response.StatusCode >= 500))
            {
                var msg = $"Call API Error: URL: {response?.RequestMessage?.RequestUri}, Request: {request}, Message: {message}, StatusCode: {response.StatusCode}";

                result.Status = OperationOutputStatus.ServerError;
                result.BusinessStatusCode = HttpStatusCode.InternalServerError.ToString();

                _logger.Log($"Error: {response.StatusCode}", msg, true);

                _logger.LogToElmah(new Exception($"Error: {msg}"));
            }
            return result;
        }

        private ApiResult<T> ConstructSuccessMessage<T>(HttpResponseMessage response, T responseContent)
        {
            string request = response.RequestMessage.Content != null ? JsonConvert.SerializeObject(response?.RequestMessage?.Content) : "";

            _logger.Log($"Success: {response.StatusCode}",
                $"Call API Success: URL: {response?.RequestMessage?.RequestUri},Request: {request}, Response: {JsonConvert.SerializeObject(responseContent)}, StatusCode:{response.StatusCode}", true);

            return new ApiResult<T>()
            {
                Result = responseContent,
                StatusCode = response.StatusCode,
                Status = OperationOutputStatus.Success
            };
        }

        private static void AppendHeaders(Dictionary<string, object> headers, HttpClient client)
        {
            try
            {
                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (headers != null)
                {
                    if (headers.Count > 0)
                    {
                        foreach (KeyValuePair<string, object> head in headers)
                        {
                            // do something with entry.Value or entry.Key
                            if (head.Key.ToLower() != "content-type")
                                client.DefaultRequestHeaders.TryAddWithoutValidation(head.Key, JsonConvert.SerializeObject(head.Value));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
