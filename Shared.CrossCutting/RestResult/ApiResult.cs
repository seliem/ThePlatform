using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Shared.CrossCutting.DomainResult;
using Shared.CrossCutting.Logging.Resources;

namespace Shared.CrossCutting.RestResult
{
    public class ApiResult<T> : BaseApiResponse
    {
        public OperationOutputStatus Status { get; set; }

        public T Result { get; set; }

        public static IHttpActionResult Success(T result, BaseApiResponse apiResult = null)
        {
            apiResult = apiResult ?? new BaseApiResponse()
            {
                StatusCode = HttpStatusCode.Created,
                BusinessStatusCode = nameof(Success)
            };
            apiResult.StatusCode = apiResult.StatusCode ?? HttpStatusCode.Created;
            apiResult.BusinessStatusCode = apiResult.BusinessStatusCode ?? nameof(Success);

            if (ValidateHttpStatusCode(apiResult.StatusCode, OperationOutputStatus.Success))
            {
                var output = new ApiResult<T>
                {
                    BusinessStatusCode = apiResult?.BusinessStatusCode,
                    MessageAr = apiResult?.MessageAr,
                    MessageEn = apiResult?.MessageEn,
                    StatusCode = apiResult?.StatusCode,
                    Result = result,
                    Status = OperationOutputStatus.Success
                };

                return new HttpActionResult<T>(output);
            }
            throw new Exception(EnglishMessages.InvalidSuccessHttpStatusCode);//"invalid success http Status Code!, failed to construct Api Result!"

        }

        public static IHttpActionResult Fail(BaseApiResponse apiResult)
        {

            apiResult = apiResult ?? new BaseApiResponse()
            {
                StatusCode = HttpStatusCode.Conflict,
                BusinessStatusCode = nameof(Fail)
            };

            apiResult.StatusCode = apiResult.StatusCode ?? HttpStatusCode.Conflict;
            apiResult.BusinessStatusCode = apiResult.BusinessStatusCode ?? nameof(Fail);

            if (ValidateHttpStatusCode(apiResult.StatusCode, OperationOutputStatus.Fail))
            {
                var output = new ApiResult<T>
                {
                    BusinessStatusCode = apiResult?.BusinessStatusCode,
                    MessageAr = apiResult?.MessageAr,
                    MessageEn = apiResult?.MessageEn,
                    StatusCode = apiResult?.StatusCode,
                    Status = OperationOutputStatus.Fail
                };
                return new HttpActionResult<T>(output);
            }
            throw new Exception(EnglishMessages.InvalidFailureHttpStatusCode);//"invalid failure http Status Code!, failed to construct Api Result!"

        }

        public static IHttpActionResult Fail(T result, BaseApiResponse apiResult)
        {

            apiResult = apiResult ?? new BaseApiResponse()
            {
                StatusCode = HttpStatusCode.Conflict,
                BusinessStatusCode = nameof(Fail)
            };

            apiResult.StatusCode = apiResult.StatusCode ?? HttpStatusCode.Conflict;
            apiResult.BusinessStatusCode = apiResult.BusinessStatusCode ?? nameof(Fail);

            if (ValidateHttpStatusCode(apiResult.StatusCode, OperationOutputStatus.Fail))
            {
                var output = new ApiResult<T>
                {
                    Result = result,
                    BusinessStatusCode = apiResult?.BusinessStatusCode,
                    MessageAr = apiResult?.MessageAr,
                    MessageEn = apiResult?.MessageEn,
                    StatusCode = apiResult?.StatusCode,
                    Status = OperationOutputStatus.Fail
                };
                return new HttpActionResult<T>(output);
            }
            throw new Exception(EnglishMessages.InvalidFailureHttpStatusCode);//"invalid failure http Status Code!, failed to construct Api Result!"
        }

        public static IHttpActionResult ServerError(Exception ex, BaseApiResponse apiResult)
        {

            apiResult = apiResult ?? new BaseApiResponse()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                BusinessStatusCode = nameof(ServerError)
            };

            apiResult.StatusCode = apiResult.StatusCode ?? HttpStatusCode.InternalServerError;
            apiResult.BusinessStatusCode = apiResult.BusinessStatusCode ?? nameof(ServerError);
            apiResult.MessageAr = apiResult.MessageAr ?? ex?.Message;
            apiResult.MessageEn = apiResult.MessageEn ?? ex?.Message;

            if (ValidateHttpStatusCode(apiResult.StatusCode, OperationOutputStatus.ServerError))
            {
                var output = new ApiResult<T>
                {
                    BusinessStatusCode = apiResult?.BusinessStatusCode,
                    MessageAr = apiResult?.MessageAr,
                    MessageEn = apiResult?.MessageEn,
                    StatusCode = apiResult?.StatusCode,
                    Status = OperationOutputStatus.ServerError
                };
                return new HttpActionResult<T>(output);

            }
            throw new Exception(EnglishMessages.InvalidServerErrorHttpStatusCode);//"invalid ServerError http Status Code!, failed to construct Api Result!"

        }

        private static bool ValidateHttpStatusCode(HttpStatusCode? statusCode, OperationOutputStatus outputStatus)
        {
            switch (outputStatus)
            {
                case OperationOutputStatus.Success:
                    return ((int)statusCode >= 200) && ((int)statusCode <= 299);

                case OperationOutputStatus.Fail:
                    return ((int)statusCode >= 400) && ((int)statusCode <= 499);
                     
                case OperationOutputStatus.ServerError:
                    return ((int)statusCode >= 500);

                default:
                    throw new Exception(EnglishMessages.UnknownHttpStatusCode);//"Unknown http Status Code!, construct Api Result failed!"
            }
        }
    }
}
