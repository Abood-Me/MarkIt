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
        private readonly IClientDataStore _store;

        public AuthService(HttpClient client, IClientDataStore store)
        {
            _client = client;
            _store = store;
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

            await _store.AddLoginDataAsync(content.Response!);

            return content.Succeeded;
        }

        public Task<bool> RegisterAsync(string username, string password, string email)
        {
            throw new NotImplementedException();
        }
    }
}
