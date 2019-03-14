using System;
namespace test_OVD_clientless.Exceptions
{
    public class UserInitializationException : Exception
    {
        public UserInitializationException(string Message) : base(Message)
        {
        }
    }
}
