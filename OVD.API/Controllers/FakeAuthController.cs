using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IConfiguration _config;
        public FakeAuthController(IFakeAuthRepository repo, IConfiguration config)
        {
            _config = config;
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

        [HttpPost("login")]
        public async Task<ActionResult> login(PreFakeLoginDto preFakeLoginDto)
        {
            var UserFromRepo = await _repo.Login(preFakeLoginDto.Username.ToLower(), preFakeLoginDto.Password);

            if (UserFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, UserFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, UserFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}