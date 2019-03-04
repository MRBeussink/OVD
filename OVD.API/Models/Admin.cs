namespace test_OVD_clientless.Models
{
    public class Admin
    {
        public string username { get; set; }
	    public byte[] passwordHash { get; set; }
	    public byte[] passwordSalt { get; set; }
    }
}
	