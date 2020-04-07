using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Shared.CrossCutting.Security
{
    public class LoggedInUserService : ILoggedInUserService
    {
        public AuthorizationModel CurrentUser
        {
            get
            {
                AuthorizationModel authModel = null;

                var authHeader = HttpContext.Current?.Request?.Headers?.GetValues("authorization");

                authHeader= authHeader ?? HttpContext.Current?.Request?.Headers?.GetValues("authorization-metaData");

                if (authHeader != null)
                {
                    authModel = JsonConvert.DeserializeObject<AuthorizationModel>(
                      AppSettings.IsEncodedHeader ? HttpUtility.UrlDecode(authHeader.First()) :
                      authHeader.First());
                }
                return authModel;
            }
        }
    }
}