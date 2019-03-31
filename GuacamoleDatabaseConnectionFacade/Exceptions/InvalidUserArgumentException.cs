using System;

namespace GuacamoleDatabaseConnectionFacade.Exceptions
{
    public class InvalidUserArgumentException : Exception
    {
        public InvalidUserArgumentException(string message) : base(message)
        {
        }
    }
}
