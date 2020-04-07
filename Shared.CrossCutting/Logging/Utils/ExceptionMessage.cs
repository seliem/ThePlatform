using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.CrossCutting.Logging.Resources;

namespace Shared.CrossCutting.Logging
{
    public static class ExceptionMessage
    {
        public static string GetErrorMessage(Exception ex, string Language)
        {

            if (ex.InnerException != null)
            {
                if (ex.InnerException.GetType().Name == ErrorCodes.ArgumentException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.ArgumentException : EnglishMessages.ArgumentException;
                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.ArgumentNullException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.ArgumentNullException : EnglishMessages.ArgumentNullException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.ArgumentOutOfRangeException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.ArgumentOutOfRangeException : EnglishMessages.ArgumentOutOfRangeException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.DivideByZeroException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.DivideByZeroException : EnglishMessages.DivideByZeroException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.FileNotFoundException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.FileNotFoundException : EnglishMessages.FileNotFoundException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.FormatException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.FormatException : EnglishMessages.FormatException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.IndexOutOfRangeException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.IndexOutOfRangeException : EnglishMessages.IndexOutOfRangeException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.InvalidOperationException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.InvalidOperationException : EnglishMessages.InvalidOperationException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.InvalidCastException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.InvalidCastException : EnglishMessages.InvalidCastException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.KeyNotFoundException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.KeyNotFoundException : EnglishMessages.KeyNotFoundException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.NotSupportedException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.NotSupportedException : EnglishMessages.NotSupportedException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.NullReferenceException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.NullReferenceException : EnglishMessages.NullReferenceException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.OverflowException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.OverflowException : EnglishMessages.OverflowException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.OutOfMemoryException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.OutOfMemoryException : EnglishMessages.OutOfMemoryException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.StackOverflowException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.StackOverflowException : EnglishMessages.StackOverflowException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.TimeoutException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.TimeoutException : EnglishMessages.TimeoutException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.SqlException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.SqlException : EnglishMessages.SqlException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.EntityException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.EntityException : EnglishMessages.EntityException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.DbEntityValidationException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.DbEntityValidationException : EnglishMessages.DbEntityValidationException;

                }
                else if (ex.InnerException.GetType().Name == ErrorCodes.UriFormatException.ToString())
                {

                    return Language.ToLower() == "ar" ? ArabicMessages.UriFormatException : EnglishMessages.UriFormatException;

                }

                else
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.GeneralMessageError : EnglishMessages.GeneralMessageError;

                }
            }
            else
            {
                if (ex.GetType().Name == ErrorCodes.ArgumentException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.ArgumentException : EnglishMessages.ArgumentException;
                }
                else if (ex.GetType().Name == ErrorCodes.ArgumentNullException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.ArgumentNullException : EnglishMessages.ArgumentNullException;

                }
                else if (ex.GetType().Name == ErrorCodes.ArgumentOutOfRangeException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.ArgumentOutOfRangeException : EnglishMessages.ArgumentOutOfRangeException;

                }
                else if (ex.GetType().Name == ErrorCodes.DivideByZeroException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.DivideByZeroException : EnglishMessages.DivideByZeroException;

                }
                else if (ex.GetType().Name == ErrorCodes.FileNotFoundException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.FileNotFoundException : EnglishMessages.FileNotFoundException;

                }
                else if (ex.GetType().Name == ErrorCodes.FormatException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.FormatException : EnglishMessages.FormatException;

                }
                else if (ex.GetType().Name == ErrorCodes.IndexOutOfRangeException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.IndexOutOfRangeException : EnglishMessages.IndexOutOfRangeException;

                }
                else if (ex.GetType().Name == ErrorCodes.InvalidOperationException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.InvalidOperationException : EnglishMessages.InvalidOperationException;

                }
                else if (ex.GetType().Name == ErrorCodes.InvalidCastException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.InvalidCastException : EnglishMessages.InvalidCastException;

                }
                else if (ex.GetType().Name == ErrorCodes.KeyNotFoundException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.KeyNotFoundException : EnglishMessages.KeyNotFoundException;

                }
                else if (ex.GetType().Name == ErrorCodes.NotSupportedException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.NotSupportedException : EnglishMessages.NotSupportedException;

                }
                else if (ex.GetType().Name == ErrorCodes.NullReferenceException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.NullReferenceException : EnglishMessages.NullReferenceException;

                }
                else if (ex.GetType().Name == ErrorCodes.OverflowException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.OverflowException : EnglishMessages.OverflowException;

                }
                else if (ex.GetType().Name == ErrorCodes.OutOfMemoryException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.OutOfMemoryException : EnglishMessages.OutOfMemoryException;

                }
                else if (ex.GetType().Name == ErrorCodes.StackOverflowException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.StackOverflowException : EnglishMessages.StackOverflowException;

                }
                else if (ex.GetType().Name == ErrorCodes.TimeoutException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.TimeoutException : EnglishMessages.TimeoutException;

                }
                else if (ex.GetType().Name == ErrorCodes.SqlException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.SqlException : EnglishMessages.SqlException;

                }
                else if (ex.GetType().Name == ErrorCodes.EntityException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.EntityException : EnglishMessages.EntityException;

                }
                else if (ex.GetType().Name == ErrorCodes.DbEntityValidationException.ToString())
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.DbEntityValidationException : EnglishMessages.DbEntityValidationException;

                }
                else if (ex.GetType().Name == ErrorCodes.UriFormatException.ToString())
                {

                    return Language.ToLower() == "ar" ? ArabicMessages.UriFormatException : EnglishMessages.UriFormatException;

                }

                else
                {
                    return Language.ToLower() == "ar" ? ArabicMessages.GeneralMessageError : EnglishMessages.GeneralMessageError;

                }

            }


        }
    }
}
