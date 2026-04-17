using System.Globalization;

namespace UNITYPOS_API.Middlewares.Exceptions
{
    public class ExceptionHelpers
    {


        public class AppExceptions : Exception
#pragma warning restore S3376 // Attribute, EventArgs, and Exception type names should end with the type being extended
        {
            public AppExceptions() : base() { }

            public AppExceptions(string message) : base(message) { }

            public AppExceptions(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
        }
    }
}
