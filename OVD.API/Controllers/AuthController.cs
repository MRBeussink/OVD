using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
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
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        // 
        [HttpPost("register")]
        public async Task<IActionResult> Register(AdminForRegisterDto adminForRegisterDto)
        {
            // validate request

            adminForRegisterDto.Username = adminForRegisterDto.Username.ToLower();

            if (await _repo.AdminExists(adminForRegisterDto.Username))
                return BadRequest("Username already exists");

            var adminToCreate = new Admin
            {
                Username = adminForRegisterDto.Username
            };

            var createdUser = await _repo.Register(adminToCreate, adminForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            // Check if dawgtag or not
            // SIU85[0-9]{7}

            Claim idClaim;
            Claim nameClaim;
            Claim roleClaim;

            Regex dawgtagRx = new Regex("[Ss][Ii][Uu]85[0-9]{7}", RegexOptions.Compiled);


            if (dawgtagRx.IsMatch(userForLoginDto.Username))
            {
                // LDAP login
                LdapAuth ldapAuth = new LdapAuth();

                // Validate user via LDAP
                var valid = ldapAuth.validateUser(userForLoginDto);

                // if invalid
                if (!valid)
                    return Unauthorized();

                // Assign security claims
                idClaim = new Claim(ClaimTypes.NameIdentifier, userForLoginDto.Username);
                nameClaim = new Claim(ClaimTypes.Name, userForLoginDto.Username);
                roleClaim = new Claim(ClaimTypes.Role, "standard");
            }

            else 
            {   
                // Admin login

                var adminFromRepo = await _repo
                    .Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

                if (adminFromRepo == null)
                    return Unauthorized();

                
                idClaim = new Claim(ClaimTypes.NameIdentifier, adminFromRepo.Id.ToString());
                nameClaim = new Claim(ClaimTypes.Name, adminFromRepo.Username);
                roleClaim = new Claim(ClaimTypes.Role, "admin");
            }

            var claims = new [] 
            {
                idClaim,
                nameClaim,
                roleClaim
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

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
