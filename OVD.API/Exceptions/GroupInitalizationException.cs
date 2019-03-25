using System;
namespace OVD.API.Exceptions
{
    public class GroupInitalizationException : Exception
    {
        public GroupInitalizationException(string Message) : base(Message)
        {
        }
    }
}
