using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Api.Models;

namespace TodoList.Api.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItem>> GetActiveTodoItems();
        Task<TodoItem> GetTodoItemById(Guid id);
        Task<bool> UpdateTodoItem(TodoItem updateTodoItem);
        (bool, string) CanCreateTodoItem(NewTodoItem newTodoItem);
        Task CreateTodoItem(TodoItem newTodoItem);
    }
}
