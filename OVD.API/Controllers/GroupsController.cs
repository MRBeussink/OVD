using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OVD.API.Models;
using OVD.API.Data;
using OVD.API.Dtos;

namespace OVD.API.Controllers
{
	// http://localhost:5000/api/groups
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupRepository _repo;
        public GroupsController(IGroupRepository repo)
        {
            _repo = repo;
        }

        // {
		// 	"name": "Test",
		// 	"vmchoice": "Windows",
		// 	"maxvms": "1",
		// 	"minvms": "1",
		// 	"numhotspares": "1",
		// 	"dawgtags": ["SIU853249208", "SIU853249209"]
		// }
        [HttpPost("create")]
        public async Task<IActionResult> createGroup(GroupForCreationDto groupForCreationDto)
        {
        	if (groupForCreationDto.Name == null)
                return BadRequest("Name is null");

            if (groupForCreationDto.Image == null)
                return BadRequest("VmChoice is null");

            if (groupForCreationDto.Total <= 0)
            	return BadRequest("MaxVms is less than 1");

            if (groupForCreationDto.Online < 0)
            	return BadRequest("MinVms is negative");

            if (groupForCreationDto.Hotspares < 0)
            	return BadRequest("NumHotspares is negative");

            if (await _repo.GroupExists(groupForCreationDto.Name))
                return BadRequest("Group already exists");

            var GroupToCreate = new Group {
                Name = groupForCreationDto.Name,
                Image = groupForCreationDto.Image,
                Total = groupForCreationDto.Total,
                Online = groupForCreationDto.Online,
                Hotspares = groupForCreationDto.Hotspares
            };

            _repo.Add(GroupToCreate);

            return Ok("Created.");
        }

        [HttpGet("groups")]
        public async Task<IActionResult> getGroups()
        {
            
             var groups = await _repo.GetGroups();

            return Ok(groups);
        }
    }
}