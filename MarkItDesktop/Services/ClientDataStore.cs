using MarkIt.Common.Models;
using MarkItDesktop.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MarkItDesktop.Services
{
    public class ClientDataStore : IClientDataStore
    {
        private readonly ClientDbContext _dbContext;

        public ClientDataStore(ClientDbContext dbContext)
        {
            _dbContext = dbContext;
            // TODO : Lazy work around, fix this later
            _dbContext.Database.Migrate();
        }
        public async Task AddLoginDataAsync(LoginResponseModel model)
        {
            // Make sure database is there.
            await _dbContext.Database.MigrateAsync();

            _dbContext.Data.Add(
                new ClientData()
                {
                    Username = model.Username,
                    Email = model.Email,
                    Token = model.Token
                });

            await _dbContext.SaveChangesAsync();
        }

        public Task<ClientData?> GetStoredLoginAsync()
        {
            return _dbContext.Data.FirstOrDefaultAsync();
        }

        public async Task<bool> HasStoredLogin()
        {
            return await GetStoredLoginAsync() != null;
        }

        public async Task ClearAllStoredLoginsAsync()
        {
            _dbContext.Data.RemoveRange(_dbContext.Data);

            await _dbContext.SaveChangesAsync();
        }

    }
}
