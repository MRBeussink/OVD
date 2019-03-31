using System;

namespace GuacamoleDatabaseConnectionFacade.Exceptions
{
    public class GroupInitalizationException : Exception
    {
        public GroupInitalizationException(string Message) : base(Message)
        {
        }
    }
}
