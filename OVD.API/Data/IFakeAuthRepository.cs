using System.Threading.Tasks;
using OVD.API.Models;

namespace OVD.API.Data
{
    public interface IFakeAuthRepository
    {
         Task<FakeUser> Register(FakeUser fakeUser, string password);
         Task<FakeUser> Login(string username, string password);
         Task<bool> isUser(string username);
         Task<bool> isAdmin(string username);


    }
}