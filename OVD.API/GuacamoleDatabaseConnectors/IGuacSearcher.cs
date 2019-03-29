using System;
using System.Collections.Generic;

namespace OVD.API.GuacamoleDatabaseConnectors
{
    public interface IGuacSearcher
    {
		bool searchGroupName(string groupName, ref List<Exception> exceptions);
		bool searchUserName(string dawgtag, ref List<Exception> exceptions);
		bool searchVmName(string vmName, ref List<Exception> exceptions);
		int getMaxVmId(ref List<Exception> exceptions);
		Queue<string> getAllGroupVmNames(string groupName,
			ref List<Exception> exceptions);
		Queue<string> getAllGroupNames(ref List<Exception> exceptions);
    }
}