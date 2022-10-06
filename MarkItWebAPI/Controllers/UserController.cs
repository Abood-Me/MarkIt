using MarkIt.Common.Models;
using MarkItWebAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MarkItWebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPatch]
        public async Task<ActionResult<APIResponseModel<UserApiModel>>> UpdateUsername([FromBody] UserApiModel model)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Forbid();

            if (model.Username is not null)
                user.UserName = model.Username;
            if(model.FullName is not null)
                user.FullName = model.FullName;
            if (model.Email is not null)
                user.Email = model.Email;

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new APIResponseModel<UserApiModel>()
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(e => e.Description).ToArray()
                });
            }


            return Ok(new APIResponseModel<UserApiModel>()
            {
                Succeeded = true,
                Response = new UserApiModel()
                {
                    Username = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                }
            });
        }

        [HttpPatch("password")]
        public async Task<ActionResult<APIResponseModel<bool>>> UpdatePassword([FromBody] UserPasswordApiModel model)
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Forbid();

            IdentityResult result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new APIResponseModel<bool>()
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(e => e.Description).ToArray()
                });
            }


            return Ok(new APIResponseModel<bool>()
            {
                Succeeded = true,
            });
        }
    }
}
