using System;

namespace Shared.Domain.Model.Extensions
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    sealed public class BusinessTypeAttribute : Attribute
    {
        readonly string _classType;

        // This is a positional argument
        public BusinessTypeAttribute(string classType)
        {
            this._classType = classType;

        }

        public string ClassType
        {
            get { return _classType; }
        }

    }
}
