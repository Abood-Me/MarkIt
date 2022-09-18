using Microsoft.AspNetCore.Identity;

namespace MarkItWebAPI.Data
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Todo> Todos { get; set; } = null!;
    }
}
