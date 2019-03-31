using System;

namespace GuacamoleDatabaseConnectionFacade.Exceptions
{
    public class InvalidDatabaseArgumentException : Exception
    {
        public InvalidDatabaseArgumentException(string Message) : base(Message)
        {
        }
    }
}
