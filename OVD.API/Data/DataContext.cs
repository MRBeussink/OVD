using Microsoft.EntityFrameworkCore;
using OVD.API.Models;

namespace OVD.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options) {}

        // Adds a table of fake basic users for testing the UI
        public DbSet<FakeUser> FakeUsers { get; set; }
        //Adds a tabe of fake admins for testing the UI
        public DbSet<FakeAdmin> FakeAdmins { get; set; }
    }
}