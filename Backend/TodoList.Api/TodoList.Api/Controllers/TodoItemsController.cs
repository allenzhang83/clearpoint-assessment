using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TodoList.Api.Models;
using TodoList.Api.Services;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly ILogger<TodoItemsController> _logger;

        public TodoItemsController(ITodoService todoService, ILogger<TodoItemsController> logger)
        {
            _todoService = todoService;
            _logger = logger;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            var results = await _todoService.GetActiveTodoItems();
            return Ok(results);
        }

        // GET: api/TodoItems/...
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(Guid id)
        {
            var result = await _todoService.GetTodoItemById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/TodoItems/... 
        [HttpPut]
        public async Task<IActionResult> PutTodoItem(TodoItem todoItem)
        {
            var todoItemExist = await _todoService.UpdateTodoItem(todoItem);

            if (!todoItemExist)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/TodoItems 
        [HttpPost]
        public async Task<IActionResult> PostTodoItem(NewTodoItem newTodoItem)
        {
            var canCreateTodoItem = _todoService.CanCreateTodoItem(newTodoItem);
            if (!canCreateTodoItem.Item1)
            {
                return BadRequest(canCreateTodoItem.Item2);
            }

            var todoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Description = newTodoItem.Description,
                IsCompleted = false
            };

            await _todoService.CreateTodoItem(todoItem);

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }
    }
}
