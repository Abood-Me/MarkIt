using System.ComponentModel.DataAnnotations;

namespace MarkItWebAPI.Models
{
    public class RegisterApiModel
    {
        [Required]
        public string Username { get; set; } = null!;
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
