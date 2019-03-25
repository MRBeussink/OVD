using System;
namespace OVD.API.Exceptions
{
    public class InvalidUserArgumentException : Exception
    {
        public InvalidUserArgumentException(string message) : base(message)
        {
        }
    }
}
