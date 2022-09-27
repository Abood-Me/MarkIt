using MarkIt.Common.Models;
using MarkItWebAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MarkItWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configurations;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configurations)
        {
            this._userManager = userManager;
            this._configurations = configurations;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterApiModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // TODO : Server side validations (login and register )!
            // TODO : Add fullname to user
            ApplicationUser user = new()
            {
                UserName = model.Username,
                Email = model.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            
            if(!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponseModel<RegisterResponseModel>()
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(e => e.Description).ToArray()
                });
            }

            // TODO : Implement Register on client side & Return Response API Model
            return Ok(new APIResponseModel<RegisterResponseModel>()
            {
                Succeeded = true,
                Response = new RegisterResponseModel()
                {
                    Username = user.UserName
                }
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser? user = await _userManager.FindByNameAsync(model.Username);

            if (user is null)
                return NotFound();

            bool valid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!valid)
                return Forbid();

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Audience = _configurations["JWT:Audience"],
                Issuer = _configurations["JWT:Issuer"],
                Expires = DateTime.Now.AddDays(15),
                Subject = new ClaimsIdentity(new[]
                 {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                 SigningCredentials = new SigningCredentials(new SymmetricSecurityKey( Encoding.UTF8.GetBytes( _configurations["JWT:Key"]) ), SecurityAlgorithms.HmacSha256)
            };
            
            JwtSecurityTokenHandler tokenHandler = new();
            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return Ok(
                new APIResponseModel<LoginResponseModel>
                {
                    Succeeded = true,
                    Response = new()
                    {
                        Username = user.UserName,
                        Email = user.Email,
                        Token = tokenHandler.WriteToken(token)
                    },
                });
        }
    }
}
