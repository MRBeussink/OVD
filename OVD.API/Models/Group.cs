using System.Collections.Generic;

namespace test_OVD_clientless.Models
{
    public class Group
    {
     	public string name { get; set; }
	    public Config config { get; set; }
	    public ICollection<User> members { get; set; }
    }	
}	