using System.Collections.Generic;

namespace test_OVD_clientless.Models
{
    public class User
    {
        public string dawgtag { get; set; }
        public ICollection<Group> groups { get; set; }
    }
}