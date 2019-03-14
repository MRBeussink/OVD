using System;
namespace test_OVD_clientless.Exceptions
{
    public class InvalidDatabaseArgumentException : Exception
    {
        public InvalidDatabaseArgumentException(string Message) : base(Message)
        {
        }
    }
}
