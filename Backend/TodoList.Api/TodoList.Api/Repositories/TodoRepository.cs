using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Models;

namespace TodoList.Api.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;

        public TodoRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItems(bool isCompleted)
        {
            return await _context.TodoItems.Where(x => x.IsCompleted == isCompleted).ToListAsync();
        }

        public async Task<TodoItem> GetTodoItemById(Guid id)
        {
            // use FirstOrDefaultAsync instead of FindAsync in case need to call Include in the future
            // https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/crud?view=aspnetcore-6.0#ways-to-read-one-entity
            return await _context.TodoItems.FirstOrDefaultAsync(i => i.Id == id);
        }

        // retrieve and update instead of changing the State
        // this generates the SQL query to only update affected fields
        public async Task<bool> UpdateTodoItem(TodoItem updateTodoItem)
        {
            var existingTodoItem = await _context.TodoItems.FirstOrDefaultAsync(i => i.Id == updateTodoItem.Id);
            if (existingTodoItem == null)
            {
                return false;
            }
            existingTodoItem.Description = updateTodoItem.Description;
            existingTodoItem.IsCompleted = updateTodoItem.IsCompleted;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task CreateTodoItem(TodoItem createTodoItem)
        {
            _context.TodoItems.Add(createTodoItem);
            await _context.SaveChangesAsync();
        }

        public bool TodoItemDescriptionExists(string description)
        {
            return _context.TodoItems
                   .Any(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted);
        }
    }
}
