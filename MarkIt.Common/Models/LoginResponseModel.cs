using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkIt.Common.Models
{
    public class LoginResponseModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }

    }
}
