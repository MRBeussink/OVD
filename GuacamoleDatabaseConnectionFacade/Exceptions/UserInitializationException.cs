using System;

namespace GuacamoleDatabaseConnectionFacade.Exceptions
{
    public class UserInitializationException : Exception
    {
        public UserInitializationException(string Message) : base(Message)
        {
        }
    }
}
