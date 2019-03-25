using System;
namespace OVD.API.Exceptions
{
    public class VmInitializationException : Exception
    {
        public VmInitializationException(string Message) : base(Message)
        {
        }
    }
}
