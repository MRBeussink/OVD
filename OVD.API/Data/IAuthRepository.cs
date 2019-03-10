using System.Threading.Tasks;
using OVD.API.Models;

namespace OVD.API.Data
{
    public interface IAuthRepository
    {
         Task<Admin> Register(Admin admin, string password);
         Task<Admin> Login(string username, string password);
         Task<bool> AdminExists(string username);
    }
}