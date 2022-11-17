using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Api.Models;
using TodoList.Api.Repositories;

namespace TodoList.Api.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<IEnumerable<TodoItem>> GetActiveTodoItems()
        {
            return await _todoRepository.GetTodoItems(false);
        }

        public async Task<TodoItem> GetTodoItemById(Guid id)
        {
            return await _todoRepository.GetTodoItemById(id);
        }

        public async Task<bool> UpdateTodoItem(TodoItem updateTodoItem)
        {
            return await _todoRepository.UpdateTodoItem(updateTodoItem);
        }

        public (bool, string) CanCreateTodoItem(NewTodoItem newTodoItem)
        {
            if (string.IsNullOrEmpty(newTodoItem?.Description))
            {
                return (false, Constants.DesctiptionRequiredMessage);
            }

            if (_todoRepository.TodoItemDescriptionExists(newTodoItem.Description))
            {
                return (false, Constants.DesctiptionExistsMessage);
            }

            return (true, string.Empty);
        }

        public async Task CreateTodoItem(TodoItem newTodoItem)
        {
            await _todoRepository.CreateTodoItem(newTodoItem);
        }
    }
}
