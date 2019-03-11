using System.Collections.Generic;

namespace test_OVD_clientless.Models
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