using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Api.Models;

namespace TodoList.Api.Repositories
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItem>> GetTodoItems(bool isCompleted);
        Task<TodoItem> GetTodoItemById(Guid id);
        Task<bool> UpdateTodoItem(TodoItem updateTodoItem);
        Task CreateTodoItem(TodoItem createTodoItem);
        bool TodoItemDescriptionExists(string description);
    }
}
