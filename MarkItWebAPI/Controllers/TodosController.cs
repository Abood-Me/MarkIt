using MarkIt.Common.Models;
using MarkItWebAPI.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MarkItWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodosController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public TodosController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo([FromBody] TodoApiModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string? id = _userManager.GetUserId(User);
            ApplicationUser? user = await _userManager.Users
                                        .Include(u => u.Todos)
                                        .FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
                return NotFound("User not found");


            Todo todo = new Todo()
            {
                Text = model.Text,
                IsCompleted = model.IsCompleted
            };

            user.Todos.Add(todo);

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);


            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Todo>> UpdateTodo(int id, [FromBody] TodoApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string? userId = _userManager.GetUserId(User);
            ApplicationUser? user = await _userManager.Users
                                        .Include(u => u.Todos)
                                        .FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                return NotFound("User not found");

            Todo? todo = user.Todos.FirstOrDefault(t => t.Id == id);
            if (todo is null)
                return NotFound("Todo not found");
            todo.IsCompleted = model.IsCompleted;
            todo.Text = model.Text;


            IdentityResult result = await _userManager.UpdateAsync(user);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> RemoveTodo([FromRoute] int id)
        {
            string? userId = _userManager.GetUserId(User);
            ApplicationUser? user = await _userManager.Users
                                        .Include(u => u.Todos)
                                        .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                return NotFound("User not found");
            Todo? todo = user.Todos.FirstOrDefault(t => t.Id == id);

            if (todo is null)
                return NotFound("Todo not found");

            user.Todos.Remove(todo);

            IdentityResult result = await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Todo>> GetTodo([FromRoute] int id)
        {
            string? userId = _userManager.GetUserId(User);
            ApplicationUser? user = await _userManager.Users
                                        .Include(u => u.Todos)
                                        .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                return NotFound("User not found");

            Todo? todo = user.Todos.FirstOrDefault(t => t.Id == id);

            if (todo is null)
                return NotFound("Todo not found");

            return Ok(todo);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            string? id = _userManager.GetUserId(User);
            ApplicationUser? user = await _userManager.Users
                                        .Include(u => u.Todos)
                                        .FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
                return NotFound("User not found");

            return Ok(user.Todos);
        }
    }
}
