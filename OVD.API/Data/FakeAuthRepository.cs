using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OVD.API.Models;

namespace OVD.API.Data
{
    public class FakeAuthRepository : IFakeAuthRepository
    {
        private readonly DataContext _context;
        public FakeAuthRepository(DataContext context)
        {
            _context = context;
        }
        // Registers a fake user
        public async Task<FakeUser> Register(FakeUser fakeUser, string password)
        {
            fakeUser.Password = password;

            await _context.FakeUsers.AddAsync(fakeUser);
            await _context.SaveChangesAsync();

            return fakeUser;
        }
        // Attempts to fake login a user or admin
        public async Task<FakeUser> Login(string username, string password)
        {
            /* This is incomplete because we can't return an admin
            if (await isAdmin(username)) 
            {
                var admin = await _context.FakeAdmins.FirstOrDefaultAsync(x => x.username == username);
                if(password == admin.password) 
                    return null;
                return admin;
            } */
            // If the supplied login credentials is a user, then login as a user
            if( await isUser(username))
            {
                var user = await _context.FakeUsers.FirstOrDefaultAsync(x => x.Username == username);
                if(password == user.Password) 
                    return null;
                return user;
            }
            return null;
        }
        // Checks if the username is an admins
        public async Task<bool> isAdmin(string username)
        {
            if (await _context.FakeAdmins.AnyAsync(x => x.Username == username))
                return true;
            return false;
        }
        // Checks if the username is a users
        public async Task<bool> isUser(string username)
        {
            if (await _context.FakeUsers.AnyAsync(x => x.Username == username))
                return true;
            return false;
        }
    }
}