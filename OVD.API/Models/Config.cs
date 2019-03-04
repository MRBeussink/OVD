using System.Collections.Generic;

namespace test_OVD_clientless.Models
{
    public class Config
    {
        public Group group { get; set; }
        public int hotspareNum { get; set; }
	    public int maxNum { get; set; }
	    public int minNum { get; set; }
        public ICollection<VirtualMachine> virtualMachines { get; set; }
    }	
}