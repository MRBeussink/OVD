using System;
namespace OVD.API.Exceptions
{
    public class InvalidDatabaseArgumentException : Exception
    {
        public InvalidDatabaseArgumentException(string Message) : base(Message)
        {
        }
    }
}
