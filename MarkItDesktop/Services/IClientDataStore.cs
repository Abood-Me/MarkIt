using MarkIt.Common.Models;
using MarkItDesktop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkItDesktop.Services
{
    public interface IClientDataStore
    {
        Task<ClientData?> GetStoredLoginAsync();

        Task<bool> HasStoredLogin();

        Task AddLoginDataAsync(LoginResponseModel model);

        Task ClearAllStoredLoginsAsync();

        Task EnsureCreated();
    }
}
