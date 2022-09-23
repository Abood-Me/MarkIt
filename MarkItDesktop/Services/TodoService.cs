using MarkIt.Common.Models;
using MarkItDesktop.Data;
using Microsoft.EntityFrameworkCore;
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
        private readonly ClientDbContext _dbContext;

        public TodoService(HttpClient client, ClientDbContext dbContext)
        {
            this._client = client;
            this._dbContext = dbContext;
        }

        public async Task<TodoResponseModel?> CreateTodoAsync(TodoApiModel model)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync(string.Empty, model);
            if (!response.IsSuccessStatusCode)
                return null;

            APIResponseModel<TodoResponseModel>? apiResponse = await response.Content.ReadFromJsonAsync<APIResponseModel<TodoResponseModel>>();

            if (!apiResponse!.Succeeded)
                return null;

            return apiResponse.Response;
        }

        public async Task<IList<TodoResponseModel>?> GetTodosAsync()
        {
            // TODO : Handle exceptions
            APIResponseModel<List<TodoResponseModel>>? response = await _client.GetFromJsonAsync<APIResponseModel<List<TodoResponseModel>>>(string.Empty);

            if (response is null || !response.Succeeded)
                return null;

            return response.Response;
        }

        public async Task<TodoResponseModel?> UpdateTodoAsync(int id, TodoApiModel model)
        {
            JsonContent content = JsonContent.Create(model, typeof(TodoApiModel));

            HttpResponseMessage response = await _client.PatchAsync($"{id}", content);
            // TODO : Response have error message, throw error when fails.
            if (!response.IsSuccessStatusCode)
                return null;

            APIResponseModel<TodoResponseModel>? apiResponse = await response.Content.ReadFromJsonAsync<APIResponseModel<TodoResponseModel>>();

            if (!apiResponse!.Succeeded)
                return null;

            return apiResponse.Response;
        }
    }
}
