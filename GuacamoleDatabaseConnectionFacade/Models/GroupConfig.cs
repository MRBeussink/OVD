using System.Collections.Generic;

namespace GuacamoleDatabaseConnectionFacade.Models
{
    public class GroupConfig
    {
        public string groupName { get; set; }
        public ICollection<VirtualMachine> virtualMachines { get; set; }
        public int hotspareNum { get; set; }
	    public int maxNum { get; set; }
	    public int minNum { get; set; }
    }	
}