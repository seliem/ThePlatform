using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Shared.CrossCutting
{
    /// <summary>
    /// 
    /// </summary>
    internal class AppSettings
    {
        internal static string ValidateAuthorizationWithAllRelationsUrl => ConfigurationManager.AppSettings[nameof(ValidateAuthorizationWithAllRelationsUrl)].ToString();
        internal static bool IsLIDMockd => (ConfigurationManager.AppSettings[nameof(IsLIDMockd)]) != null
            && (ConfigurationManager.AppSettings[nameof(IsLIDMockd)]).ToString().ToLower() == "true";

        internal static string MockedAuthModel => ConfigurationManager.AppSettings[nameof(MockedAuthModel)].ToString();

        internal static bool IsEncodedHeader => (ConfigurationManager.AppSettings[nameof(IsEncodedHeader)]) != null
             && (ConfigurationManager.AppSettings[nameof(IsEncodedHeader)]).ToString().ToLower() == "true";


    }
}