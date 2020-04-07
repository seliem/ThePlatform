using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Net;

namespace Shared.CrossCutting.DomainResult
{
    public class OperationResult<T> : BaseDomainResult
    {
        public OperationOutputStatus Status { get; set; }

        public T Result { get; set; }

        public static OperationResult<T> Success(T result, BaseDomainResult DomainResult = null)
        {
            DomainResult = DomainResult ?? new BaseDomainResult()
            {
                BusinessStatusCode = nameof(Success)
            };
            DomainResult.BusinessStatusCode = DomainResult.BusinessStatusCode ?? nameof(Success);

            return new OperationResult<T>
            {
                BusinessStatusCode = DomainResult?.BusinessStatusCode,
                MessageAr = DomainResult?.MessageAr,
                MessageEn = DomainResult?.MessageEn,
                Result = result,
                Status = OperationOutputStatus.Success
            };
        }

        public static OperationResult<T> Fail(BaseDomainResult DomainResult)
        {

            DomainResult = DomainResult ?? new BaseDomainResult()
            {
                BusinessStatusCode = nameof(Fail)
            };

            DomainResult.BusinessStatusCode = DomainResult.BusinessStatusCode ?? nameof(Fail);

            return new OperationResult<T>
            {
                BusinessStatusCode = DomainResult?.BusinessStatusCode,
                MessageAr = DomainResult?.MessageAr,
                MessageEn = DomainResult?.MessageEn,
                Status = OperationOutputStatus.Fail
            };
        }

        public static OperationResult<T> Fail(T result, BaseDomainResult DomainResult)
        {

            DomainResult = DomainResult ?? new BaseDomainResult()
            {
                BusinessStatusCode = nameof(Fail)
            };

            DomainResult.BusinessStatusCode = DomainResult.BusinessStatusCode ?? nameof(Fail);

            return new OperationResult<T>
            {
                Result = result,
                BusinessStatusCode = DomainResult?.BusinessStatusCode,
                MessageAr = DomainResult?.MessageAr,
                MessageEn = DomainResult?.MessageEn,
                Status = OperationOutputStatus.Fail
            };
        }


        public static OperationResult<T> ServerError(Exception ex, BaseDomainResult DomainResult)
        {

            DomainResult = DomainResult ?? new BaseDomainResult()
            {
                BusinessStatusCode = nameof(ServerError)
            };

            DomainResult.BusinessStatusCode = DomainResult.BusinessStatusCode ?? nameof(ServerError);
            DomainResult.MessageAr = DomainResult.MessageAr ?? ex.Message;
            DomainResult.MessageEn = DomainResult.MessageEn ?? ex.Message;

            return new OperationResult<T>
            {
                BusinessStatusCode = DomainResult?.BusinessStatusCode,
                MessageAr = DomainResult?.MessageAr,
                MessageEn = DomainResult?.MessageEn,
                Status = OperationOutputStatus.ServerError
            };
        }
    }
}
