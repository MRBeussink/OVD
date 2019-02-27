using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OVD.API.Data;
using OVD.API.Dtos;
using OVD.API.Models;

namespace OVD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakeAuthController : ControllerBase
    {
        private readonly IFakeAuthRepository _repo;
        public FakeAuthController(IFakeAuthRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(PreFakeRegisteredUserInfoDto preFakeRegisteredUserInfoDto)
        {
            preFakeRegisteredUserInfoDto.Username = preFakeRegisteredUserInfoDto.Username.ToLower();

            if (await _repo.isUser(preFakeRegisteredUserInfoDto.Username))
                return BadRequest("Username already exists");

            var preFakeRegisteredUser = new FakeUser
            {
                Username = preFakeRegisteredUserInfoDto.Username
            };

            var FakeRegisteredUser = await _repo.Register(preFakeRegisteredUser, preFakeRegisteredUserInfoDto.Password);

            return StatusCode(201);
        }
    }
}