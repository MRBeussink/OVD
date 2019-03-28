using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OVD.API.Dtos;

namespace OVD.API.Controllers
{
	// http:localhost:api/groups
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {

        // {
		// 	"name": "Test",
		// 	"vmchoice": "Windows",
		// 	"maxvms": "1",
		// 	"minvms": "1",
		// 	"numhotspares": "1",
		// 	"dawgtags": ["SIU853249208", "SIU853249209"]
		// }
        [HttpPost("create")]
        public async Task<IActionResult> createGroup(GroupDto groupDto)
        {
        	if (groupDto.Name == null)
                return BadRequest("Name is null");

            if (groupDto.VmChoice == null)
                return BadRequest("VmChoice is null");

            if (groupDto.MaxVms == 0)
            	return BadRequest("MaxVms is 0");

            if (groupDto.MinVms == 0)
            	return BadRequest("MinVms is 0");

            if (groupDto.NumHotspares == 0)
            	return BadRequest("NumHotspares is 0");

            if (groupDto.Dawgtags == null)
                return BadRequest("Dawgtags is null");

            return Ok();
        }
    }
}