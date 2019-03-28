using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OVD.API.Dtos;

namespace OVD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> createGroup(GroupDto groupDto)
        {
            if (groupDto.Dawgtags == null)
                return BadRequest();

            return Ok();
        }
    }
}