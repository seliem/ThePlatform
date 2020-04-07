using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CrossCutting.Security
{
    public class AuthorizationModel
    {

        public string ISAMSessionId { get; set; }
        public int ServiceId { get; set; }
        public int RoleId { get; set; }
        public int ActionId { get; set; }
        public long UserId { get; set; }
        public int OrganizationId { get; set; }
        public string Language { get; set; }
        public int DepartmentId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        #region non-Required info
        public string UserDisplayName { get; set; }
        public string RoleName { get; set; }
        public string OrganizationName { get; set; }
        #endregion
    }
}
