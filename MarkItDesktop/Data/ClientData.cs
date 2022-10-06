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
        public int Id { get; set; }
        [Required]
        public string? Token { get; set; }
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
    }
}
