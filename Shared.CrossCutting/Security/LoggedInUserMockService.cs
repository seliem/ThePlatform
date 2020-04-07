using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Shared.CrossCutting.Security
{
    public class LoggedInUserMockService : ILoggedInUserService
    {
        public AuthorizationModel CurrentUser
        {
            get
            {
                AuthorizationModel authModel = null;

                if (AppSettings.MockedAuthModel != null)
                {
                    authModel = JsonConvert.DeserializeObject<AuthorizationModel>(AppSettings.MockedAuthModel);
                }
                return authModel;
            }
        }
    }
}