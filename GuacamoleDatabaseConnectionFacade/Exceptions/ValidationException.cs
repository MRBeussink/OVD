using System;

namespace GuacamoleDatabaseConnectionFacade.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string Message) : base(Message)
        {
        }
    }
}
