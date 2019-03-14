using System;
namespace test_OVD_clientless.Exceptions
{
    public class InvalidUserArgumentException : Exception
    {
        public InvalidUserArgumentException(string message) : base(message)
        {
        }
    }
}
