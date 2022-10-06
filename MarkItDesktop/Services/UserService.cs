using MarkIt.Common.Models;
using MarkItDesktop.Data;
using MarkItDesktop.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MarkItDesktop.Services
{
    public class UserService : IUserService
    {
        private readonly IClientDataStore _store;
        private readonly HttpClient _client;

        public UserService(IClientDataStore store, HttpClient client)
        {
            _store = store;
            _client = client;
        }
        public async Task<bool> UpdateUser(UserApiModel user)
        {
            JsonContent requestContent = JsonContent.Create(user);
            HttpResponseMessage response = await _client.PatchAsync(string.Empty, requestContent);
            // Don't forget Json format exception
            APIResponseModel<UserApiModel>? responseModel = await response.Content.ReadFromJsonAsync<APIResponseModel<UserApiModel>>();

            if (responseModel is { } content)
            {
                if (!content.Succeeded)
                    throw new ValidationException(content.Errors?.FirstOrDefault());

                await _store.UpdateUserInfo(content.Response!);

                return true;
            }

            return false;
        }

        public async Task<bool> UpdateUserPassword(UserPasswordApiModel model)
        {
            JsonContent requestContent = JsonContent.Create(model);
            HttpResponseMessage response = await _client.PatchAsync("password", requestContent);

            APIResponseModel<bool>? responseModel = await response.Content.ReadFromJsonAsync<APIResponseModel<bool>>();

            if (responseModel is { } content && !content.Succeeded)
                throw new ValidationException(content.Errors?.FirstOrDefault());

            return response.IsSuccessStatusCode;
        }
    }
}
