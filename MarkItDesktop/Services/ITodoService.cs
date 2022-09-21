using MarkIt.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkItDesktop.Services
{
    public interface ITodoService
    {
        Task<IList<TodoResponseModel>?> GetTodosAsync();

        Task<TodoResponseModel?> CreateTodoAsync(TodoApiModel model);

        Task<TodoResponseModel?> UpdateTodoAsync(int id, TodoApiModel model);


    }
}
