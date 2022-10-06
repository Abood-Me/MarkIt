using System.ComponentModel.DataAnnotations;

namespace MarkIt.Common.Models
{
    public class UserPasswordApiModel
    {
        [Required]
        [MinLength(8)]
        public string? NewPassword { get; set; }
        public string? CurrentPassword { get; set; }
    }
}
