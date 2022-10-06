using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MarkItWebAPI.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string? FullName { get; set; } = null!;
        public virtual ICollection<Todo> Todos { get; set; } = null!;
    }
}
