using System;
namespace test_OVD_clientless.Exceptions
{
    public class VmInitializationException : Exception
    {
        public VmInitializationException(string Message) : base(Message)
        {
        }
    }
}
