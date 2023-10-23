using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using RoofingAmerica.Domain.Core.Data;
using RoofingAmerica.Domain.Models;
using RoofingAmerica.Domain.Models.Dtos;
using RoofingAmerica.Domain.Services;
using RoofingAmerica.Infrastructure.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace RoofingAmerica.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly IConfiguration _configuration;

        public AuthenticationController
            (
            UserManager<IdentityUser> userManager,
            JwtConfig jwtConfig,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _jwtConfig = jwtConfig;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(requestDto.Email);

                if (existingUser != null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Result = false,
                        Errors = new List<string>
                {
                    "That email already exists!"
                }
                    });
                }

                var newUser = new IdentityUser()
                {
                    Email = requestDto.Email,
                    UserName = requestDto.Name
                };

                var result = await _userManager.CreateAsync(newUser, requestDto.Password);

                if (result.Succeeded)
                {
                    var token = GenerateJwtToken(newUser);
                    return Ok(new AuthResult()
                    {
                        Result = true,
                        Token = token
                    });
                }
                else
                {
                    var errors = result.Errors.Select(error => error.Description).ToList();
                    return BadRequest(new AuthResult()
                    {
                        Errors = errors,
                        Result = false
                    });
                }
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto loginRequest)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(loginRequest.Email);

                if (existingUser == null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>
                        {
                            "Invalid payload!"
                        },
                        Result = false
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, loginRequest.Password);

                if (!isCorrect)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>
                        {
                            "Invalid credentials!"
                        },
                        Result = false
                    });
                }

                var jwtToken = GenerateJwtToken(existingUser);

                return Ok(new AuthResult()
                {
                    Token = jwtToken,
                    Result = true
                });
            }

            return BadRequest(new AuthResult()
            {
                Errors = new List<string>
                {
                    "Invalid payload"
                },
                Result = false
            });
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest("Failed to delete the user.");
        }


        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
                }),

                Expires = DateTime.Now.ToUniversalTime().AddHours(2),
                NotBefore = DateTime.Now.ToUniversalTime(),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
