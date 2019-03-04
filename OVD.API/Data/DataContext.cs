using Microsoft.EntityFrameworkCore;
using OVD.API.Models;

namespace OVD.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options) {}
        public DbSet<Admin> Admins { get; set; }
    }
}