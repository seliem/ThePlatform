using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CrossCutting.Security
{
    public interface ILoggedInUserService
    {
        AuthorizationModel CurrentUser { get; }
    }
}
