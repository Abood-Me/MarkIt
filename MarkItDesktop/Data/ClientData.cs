using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkItDesktop.Data
{
    public class ClientData
    {
        [Key]
        public string Id { get; set; } = null!;
        [Required]
        public string Token { get; set; } = null!;
        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}
