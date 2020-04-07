using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Shared.CrossCutting.DomainResult;
using Shared.CrossCutting.Logging.Resources;
using Shared.CrossCutting.RestOperations;
using Shared.CrossCutting.RestResult;

namespace Shared.CrossCutting.Security
{
    public class LIDAuthorize : ActionFilterAttribute
    {
        private int? ActionId { get; set; }
        private int ServiceId { set; get; }

        public LIDAuthorize(int serviceId, int actionId)
        {
            ActionId = actionId;
            ServiceId = serviceId;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                if (!AppSettings.IsLIDMockd)
                {
                    var restRequestFactory = actionContext.Request.GetDependencyScope().GetService(typeof(IRestRequestFactory)) as IRestRequestFactory;

                    AuthorizationModel authModel = null;

                    actionContext.Request.Headers.TryGetValues("authorization", out IEnumerable<string> values);

                    if ((values != null && values.Any()) || actionContext.Request.Headers.TryGetValues("authorization-metaData", out values))
                    {
                        authModel = JsonConvert.DeserializeObject<AuthorizationModel>(
                             AppSettings.IsEncodedHeader ? HttpUtility.UrlDecode(values.First()) :
                             values.First());
                    }

                    if (authModel == null)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new ApiResult<object>()
                        {
                            BusinessStatusCode = ErrorCodes.MissingAuthorizationHeader.ToString(),
                            MessageAr = ArabicMessages.MissingAuthorizationHeader.ToString(),
                            MessageEn = EnglishMessages.MissingAuthorizationHeader.ToString(),
                            StatusCode = HttpStatusCode.Unauthorized,
                            Status = OperationOutputStatus.UnAuthorized,
                        });
                        return;
                    };

                    authModel.ActionId = ActionId.HasValue ? ActionId.Value : 0;
                    authModel.ServiceId = ServiceId;

                    var result = restRequestFactory.PostResourceMessage<bool>(AppSettings.ValidateAuthorizationWithAllRelationsUrl, authModel);

                    if (result.Status != OperationOutputStatus.Success || result.StatusCode != HttpStatusCode.OK)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new ApiResult<object>()
                        {
                            BusinessStatusCode = ErrorCodes.UnAuthorized.ToString(),
                            MessageAr = ArabicMessages.NoAuthorization.ToString(),
                            MessageEn = EnglishMessages.NoAuthorization.ToString(),
                            StatusCode = HttpStatusCode.Unauthorized,
                            Status = OperationOutputStatus.UnAuthorized,
                        });
                        return;
                    }
                }

                base.OnActionExecuting(actionContext);
            }
            catch (Exception ex)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
                return;
            }
        }
    }
}