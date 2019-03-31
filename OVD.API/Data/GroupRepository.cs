using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OVD.API.Models;
using Microsoft.EntityFrameworkCore;

namespace OVD.API.Data
{
    public class GroupRepository : IGroupRepository
    {
        private readonly DataContext _context;
        public GroupRepository(DataContext context)
        {
            _context = context;

        }
        public void Add(Group group)
        {
            group.Online = 0;
            group.Active = 0;
            _context.Add(group);
        }

        public void Delete(string name)
        {
            _context.Remove(GetGroup(name));
        }

        public async Task<Group> GetGroup(string name)
        {
            var group = await _context.Groups
                .FirstOrDefaultAsync(g => g.Name == name);
            return group;
        }

        public async Task<IEnumerable<Group>> GetGroups()
        {
            var groups = await _context.Groups.ToListAsync();

            return groups;
        }

        public async Task<bool> GroupExists(string name)
        {
            return false;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}