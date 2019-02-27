namespace OVD.API.Models
{
    public class FakeAdmin
    {
        // Just a generic ID for the db
        public int Id { get; set; }
        // Username attribute of the fake user for testing UI reactions to specific logins
        public string Username { get; set; }
        // Password attribute of the fake user for testing UI reactions to bad logins
        public string Password { get; set; }
    }
}