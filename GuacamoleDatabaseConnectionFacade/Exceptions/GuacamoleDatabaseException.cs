using System;

namespace GuacamoleDatabaseConnectionFacade.Exceptions
{
    public class GuacamoleDatabaseException : Exception
    {
        public GuacamoleDatabaseException(string Message) : base(Message)
        {
        }
    }
}
