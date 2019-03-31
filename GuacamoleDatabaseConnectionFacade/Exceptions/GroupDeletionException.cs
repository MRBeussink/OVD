using System;

namespace GuacamoleDatabaseConnectionFacade.Exceptions
{
    public class GroupDeletionException : Exception
    {
        public GroupDeletionException(string Message) : base(Message)
        {
        }
    }
}
