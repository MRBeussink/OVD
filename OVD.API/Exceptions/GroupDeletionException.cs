using System;
namespace OVD.API.Exceptions
{
    public class GroupDeletionException : Exception
    {
        public GroupDeletionException(string Message) : base(Message)
        {
        }
    }
}
