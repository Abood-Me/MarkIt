using MarkIt.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkItDesktop.Services
{
    public interface IUserService
    {
        Task<bool> UpdateUser(UserApiModel user);
        Task<bool> UpdateUserPassword(UserPasswordApiModel model);
    }
}
