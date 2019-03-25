using System;
namespace OVD.API.Exceptions
{
    public class UserInitializationException : Exception
    {
        public UserInitializationException(string Message) : base(Message)
        {
        }
    }
}
