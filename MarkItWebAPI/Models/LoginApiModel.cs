using System.ComponentModel.DataAnnotations;

namespace MarkItWebAPI.Models
{
    public class LoginApiModel
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}