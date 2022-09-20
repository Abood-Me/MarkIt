using MarkIt.Common.Models;
using MarkItDesktop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MarkItDesktop.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _client;
        private readonly ClientDbContext _dbContext;

        public AuthService(HttpClient client, ClientDbContext dbContext)
        {
            this._client = client;
            this._dbContext = dbContext;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            HttpResponseMessage response =  await _client.PostAsJsonAsync<LoginApiModel>("login", new() { 
                Username = username,
                Password = password
            });

            if (!response.IsSuccessStatusCode)
                return false;

            var content = await response.Content.ReadFromJsonAsync<APIResponseModel<LoginResponseModel>>();
            if (content is null)
                return false;

            _dbContext.Data.Add(
                new ClientData()
                {
                    Username = content.Response!.Username,
                    Email = content.Response.Email,
                    Token = content.Response.Token
                });

            await _dbContext.SaveChangesAsync();

            return content.Succeeded;
        }

        public Task<bool> RegisterAsync(string username, string password, string email)
        {
            throw new NotImplementedException();
        }
    }
}
