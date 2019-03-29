using System;
using System.Collections.Generic;

namespace OVD.API.GuacamoleDatabaseConnectors
{
    public interface IGuacDeleter
    {
         bool deleteUserGroup(string groupName, ref List<Exception> exceptions);
         bool deleteConnectionGroup(string groupName, ref List<Exception> exeptions);
         // bool deleteQuery(string queryString, Queue<string> argNames,
        	// Queue<string> args, ref List<Exception> exceptions);
    }
}