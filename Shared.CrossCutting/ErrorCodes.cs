using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CrossCutting
{
    public enum ErrorCodes
    {
        GET_LOOKUPSITEMS_ERROR,
        GET_LOOKUPSITEM_ERROR,
        ArgumentException,
        ArgumentNullException,
        ArgumentOutOfRangeException,
        DbEntityValidationException,
        DivideByZeroException,
        EntityException,
        FileNotFoundException,
        FormatException,
        GeneralMessageError,
        IndexOutOfRangeException,
        InternalServerError,
        InvalidCastException,
        InvalidOperationException,
        KeyNotFoundException,
        NoAuthorization,
        NoISAMSessionKey,
        NotSupportedException,
        NullInputData,
        NullReferenceException,
        OutOfMemoryException,
        OverflowException,
        SqlException,
        StackOverflowException,
        SubjectHasRequestConnected,
        TimeoutException,
        UriFormatException,
        UnAuthorized,
        MissingAuthorizationHeader,
    }
}
