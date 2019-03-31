using System.Collections.Generic;
using System.Threading.Tasks;
using OVD.API.Models;

namespace OVD.API.Data
{
    public interface IGroupRepository
    {
         void Add(Group group);
         void Delete(string name);
         Task<bool> SaveAll();
         Task<IEnumerable<Group>> GetGroups();
         Task<Group> GetGroup(string name);
         Task<bool> GroupExists(string name);
    }
}