using System;

namespace GuacamoleDatabaseConnectionFacade.Exceptions
{
    public class VmInitializationException : Exception
    {
        public VmInitializationException(string Message) : base(Message)
        {
        }
    }
}
