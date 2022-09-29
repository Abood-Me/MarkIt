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
            if (!ModelState.IsValid)
            {
                return BadRequest(new APIResponseModel<TodoResponseModel>
                {
                    Succeeded = false,
                    Errors = ModelState.Values.Select(m => m.Errors.FirstOrDefault()?.ErrorMessage ?? string.Empty).ToArray()
                });
            }

            string? id = _userManager.GetUserId(User);

            ApplicationUser? user = await _userManager.Users
                                        .Include(u => u.Todos)
                                        .FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
                return NotFound(new APIResponseModel<TodoResponseModel>
                {
                    Succeeded = false,
                    Errors = new[]
                    {
                        "Logged in user data not found."
                    }
                });


            Todo todo = new Todo()
            {
                Text = model.Text,
                IsCompleted = model.IsCompleted
            };

            user.Todos.Add(todo);

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new APIResponseModel<TodoResponseModel>()
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(e => e.Description).ToArray()
                });


            return CreatedAtAction(
                nameof(GetTodo), 
                new { id = todo.Id }, 
                new APIResponseModel<TodoResponseModel>() { 
                    Succeeded = true,
                    Response = new TodoResponseModel()
                    {
                        Id = todo.Id,
                        Text = todo.Text,
                        IsCompleted = todo.IsCompleted
                    }
                });
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Todo>> UpdateTodo(int id, [FromBody] TodoApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new APIResponseModel<TodoResponseModel>
                {
                    Succeeded = false,
                    Errors = ModelState.Values.Select(m => m.Errors.FirstOrDefault()?.ErrorMessage ?? string.Empty).ToArray()
                });
            }

            string? userId = _userManager.GetUserId(User);
            ApplicationUser? user = await _userManager.Users
                                        .Include(u => u.Todos)
                                        .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                return NotFound(new APIResponseModel<TodoResponseModel>
                {
                    Succeeded = false,
                    Errors = new[]
                    {
                        "Logged in user data not found."
                    }
                });

            Todo? todo = user.Todos.FirstOrDefault(t => t.Id == id);
            if (todo is null)
                return NotFound(new APIResponseModel<TodoResponseModel>
                {
                    Succeeded = false,
                    Errors = new[]
                    {
                        "Requested todo not found."
                    }
                });

            todo.IsCompleted = model.IsCompleted;
            todo.Text = model.Text;


            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new APIResponseModel<TodoResponseModel>()
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(e => e.Description).ToArray()
                });

            return AcceptedAtAction(
                nameof(GetTodo),
                new { id = todo.Id },
                new APIResponseModel<TodoResponseModel>()
                {
                    Succeeded = true,
                    Response = new TodoResponseModel()
                    {
                        Id = todo.Id,
                        Text = todo.Text,
                        IsCompleted = todo.IsCompleted
                    }
                });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> RemoveTodo([FromRoute] int id)
        {
            string? userId = _userManager.GetUserId(User);
            ApplicationUser? user = await _userManager.Users
                                        .Include(u => u.Todos)
                                        .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                return NotFound(new APIResponseModel<TodoResponseModel>
                {
                    Succeeded = false,
                    Errors = new[]
                    {
                        "Logged in user data not found."
                    }
                });

            Todo? todo = user.Todos.FirstOrDefault(t => t.Id == id);

            if (todo is null)
                return NotFound(new APIResponseModel<TodoResponseModel>
                {
                    Succeeded = false,
                    Errors = new[]
                    {
                        "Requested todo not found."
                    }
                });

            user.Todos.Remove(todo);

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(new APIResponseModel<TodoResponseModel>()
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(e => e.Description).ToArray()
                });

            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<APIResponseModel<TodoResponseModel>>> GetTodo([FromRoute] int id)
        {
            string? userId = _userManager.GetUserId(User);
            ApplicationUser? user = await _userManager.Users
                                        .Include(u => u.Todos)
                                        .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                return NotFound(new APIResponseModel<TodoResponseModel>
                {
                    Succeeded = false,
                    Errors = new[]
                    {
                        "Logged in user data not found."
                    }
                });

            Todo? todo = user.Todos.FirstOrDefault(t => t.Id == id);

            if (todo is null)
                return NotFound(new APIResponseModel<TodoResponseModel>
                {
                    Succeeded = false,
                    Errors = new[]
                    {
                        "Requested todo not found."
                    }
                });

            return Ok(new APIResponseModel<TodoResponseModel>()
            {
                Succeeded = true,
                Response = new TodoResponseModel()
                {
                    Id = todo.Id,
                    Text = todo.Text,
                    IsCompleted = todo.IsCompleted
                }
            });
        }

        [HttpGet]
        public async Task<ActionResult<APIResponseModel<List<TodoResponseModel>>>> GetTodos()
        {
            string? id = _userManager.GetUserId(User);
            ApplicationUser? user = await _userManager.Users
                                        .Include(u => u.Todos)
                                        .FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
                return NotFound(new APIResponseModel<TodoResponseModel>
                {
                    Succeeded = false,
                    Errors = new[]
                    {
                        "Logged in user data not found."
                    }
                });

            return Ok(new APIResponseModel<List<TodoResponseModel>>
            {
                Succeeded = true,
                Response = new List<TodoResponseModel>( 
                    user.Todos.Select(t => 
                        new TodoResponseModel 
                        { 
                            Id = t.Id,
                            Text = t.Text, 
                            IsCompleted = t.IsCompleted 
                        })
                    )
            });
        }
    }
}
