using System;
using System.Collections.Generic;
using OVD.API.Models;
using OVD.API.Exceptions;

namespace OVD.API.GuacamoleDatabaseConnectors
{
    public interface IGuacInserter
    {
		bool insertConnectionGroup(GroupConfig newGroup, ref List<Exception> exceptions);
		bool insertUserGroup(GroupConfig newGroup, ref List<Exception> exceptions);
		bool insertUser(string dawgtag, ref List<Exception> exceptions);
		bool insertVm(string vmName, string vmChoice, ref List<Exception> exceptions);
		bool insertVmIntoConnectionGroup(VirtualMachine vm, GroupConfig group, ref List<Exception> exceptions);
		bool insertUserIntoUserGroup(string dawgtag, string groupName, ref List<Exception> exceptions);
		bool insertConnectionGroupIntoUserGroup(string groupName, ref List<Exception> exceptions);
    }
}