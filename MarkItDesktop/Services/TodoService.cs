using MarkIt.Common.Models;
using MarkItDesktop.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MarkItDesktop.Services
{
    public class TodoService : ITodoService
    {
        private readonly HttpClient _client;

        public TodoService( HttpClient client )
        {
            this._client = client;
        }

        public async Task<TodoResponseModel?> CreateTodoAsync(TodoApiModel model)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync(string.Empty, model);

            APIResponseModel<TodoResponseModel>? responseModel = await response.Content.ReadFromJsonAsync<APIResponseModel<TodoResponseModel>>();

            if (responseModel is { } content)
            {
                if (!content.Succeeded)
                    throw new NotImplementedException();

                return content.Response;
            }

            return null;
        }

        public async Task<IList<TodoResponseModel>?> GetTodosAsync()
        {
            APIResponseModel<List<TodoResponseModel>>? responseModel = await _client.GetFromJsonAsync<APIResponseModel<List<TodoResponseModel>>>(string.Empty);
            if (responseModel is { } content)
            {
                if (!content.Succeeded)
                    throw new NotImplementedException();

                return content.Response;
            }

            return null;
        }

        public async Task<TodoResponseModel?> UpdateTodoAsync(int id, TodoApiModel model)
        {
            JsonContent requestContent = JsonContent.Create(model, typeof(TodoApiModel));

            HttpResponseMessage response = await _client.PatchAsync($"{id}", requestContent);

            APIResponseModel<TodoResponseModel>? responseModel = await response.Content.ReadFromJsonAsync<APIResponseModel<TodoResponseModel>>();

            if (responseModel is { } content)
            {
                if (!content.Succeeded)
                    throw new NotImplementedException();

                return content.Response;
            }

            return null;
        }
    }
}
