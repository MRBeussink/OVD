using System.Collections.Generic;

namespace OVD.API.Models
{
    public class Workflow
    {
        public string name { get; set; }
	    public string path { get; set; }
        public ICollection<string> parameters { get; set; }
    }
}