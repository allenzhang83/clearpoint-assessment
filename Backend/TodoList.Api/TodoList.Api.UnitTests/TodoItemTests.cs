using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Api.Controllers;
using TodoList.Api.Models;
using TodoList.Api.Repositories;
using TodoList.Api.Services;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace TodoList.Api.UnitTests
{
    public class TodoItemTests
    {
        // not using moq for these tests, the logic is too simple and there're some logic in the repository that should be covered
        private List<TodoItem> _todoItems = new List<TodoItem>()
        {
            new TodoItem
            {
                Id = new Guid("77f611ad-628a-4d1d-b619-e2964227c128"),
                Description = "Item01",
                IsCompleted = false
            },
            new TodoItem
            {
                Id = new Guid("77f611ad-628a-4d1d-b619-e2964227c129"),
                Description = "Item02",
                IsCompleted = false
            },
            new TodoItem
            {
                Id = new Guid("77f611ad-628a-4d1d-b619-e2964227c12a"),
                Description = "Item03",
                IsCompleted = true
            },
            new TodoItem
            {
                Id = new Guid("77f611ad-628a-4d1d-b619-e2964227c12b"),
                Description = "Item04",
                IsCompleted = true
            },
        };

        private TodoItemsController _todoItemsController;

        private TodoContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;
            var context = new TodoContext(options);            
            context.TodoItems.AddRange(_todoItems);

            context.SaveChanges();

            return context;
        }

        private void RefreshData()
        {
            var context = GetContextWithData();
            var repositoy = new TodoRepository(context);
            var todoService = new TodoService(repositoy);
            _todoItemsController = new TodoItemsController(todoService, new Mock<ILogger<TodoItemsController>>().Object);
        }

        [Fact]
        public async Task GetTodoItems_Should_Return_All_Active_TodoItems()
        {
            // arrange
            RefreshData();

            // act
            var result = await _todoItemsController.GetTodoItems() as OkObjectResult;

            // assert            
            var todoItems = result.Value as List<TodoItem>;
            var completeItems = todoItems.Where(i => i.IsCompleted);
            var activeItems = todoItems.Where(i => !i.IsCompleted);
            completeItems.Should().HaveCount(0);
            activeItems.Should().HaveCount(2);
        }

        [Theory]
        [InlineData("77f611ad-628a-4d1d-b619-e2964227c128", "Item01")]
        // assume that the API should return completed todo item
        [InlineData("77f611ad-628a-4d1d-b619-e2964227c12a", "Item03")]
        public async Task GetTodoItem_By_Known_Id_Should_Return_TodoItem(Guid id, string expectedDescription)
        {
            // arrange
            RefreshData();

            // act
            var result = await _todoItemsController.GetTodoItem(id) as OkObjectResult;

            // assert            
            var todoItem = result.Value as TodoItem;
            todoItem.Description.Should().Be(expectedDescription);
        }

        [Fact]
        public async Task GetTodoItem_By_Unknown_Id_Should_Return_NotFound()
        {
            // arrange
            RefreshData();

            // act
            var result = await _todoItemsController.GetTodoItem(Guid.NewGuid());

            // assert 
            result.Should().BeOfType<NotFoundResult>();
        }

        [Theory]
        [InlineData("77f611ad-628a-4d1d-b619-e2964227c128", "Item01-Updated", false)]
        [InlineData("77f611ad-628a-4d1d-b619-e2964227c129", "Item02-Updated", true)]
        // assume that the API should return completed todo item
        [InlineData("77f611ad-628a-4d1d-b619-e2964227c12a", "Item03-Updated", true)]
        public async Task PutTodoItem_Should_Update_Known_TodoItem(Guid id, string expectedDescription, bool isCompleted)
        {
            // arrange
            RefreshData();
            var existingItem = (await _todoItemsController.GetTodoItem(id) as OkObjectResult).Value as TodoItem;
            existingItem.Description = existingItem.Description + "-Updated";
            existingItem.IsCompleted = isCompleted;

            // act
            var result = await _todoItemsController.PutTodoItem(existingItem);
            var updatedItem = (await _todoItemsController.GetTodoItem(id) as OkObjectResult).Value as TodoItem;

            // assert
            result.Should().BeOfType<NoContentResult>();
            updatedItem.Description.Should().Be(expectedDescription);
            updatedItem.IsCompleted.Should().Be(isCompleted);
        }

        [Fact]
        public async Task PutTodoItem_Should_Return_NotFound_For_Unknown_TodoItem()
        {
            // arrange
            RefreshData();
            var unknownItem = new TodoItem
            {
                Id = Guid.NewGuid()
            };

            // act
            var result = await _todoItemsController.PutTodoItem(unknownItem);

            // assert 
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task PostTodoItem_Should_Create_TodoItem()
        {
            // arrange
            RefreshData();
            var newItem = new NewTodoItem
            {
                Description = "Item05"
            };

            // act
            var result = await _todoItemsController.PostTodoItem(newItem);
            var getResult = await _todoItemsController.GetTodoItems() as OkObjectResult;

            // assert 
            result.Should().BeOfType<CreatedAtActionResult>();
            var todoItems = getResult.Value as List<TodoItem>;
            var addedItem = todoItems.FirstOrDefault(i => i.Description == newItem.Description);
            addedItem.Should().NotBeNull();
            addedItem.Description.Should().Be(newItem.Description);
            addedItem.IsCompleted.Should().Be(newItem.IsCompleted);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task PostTodoItem_With_Empty_Description_Should_Return_BadRequest(string description)
        {
            // arrange
            RefreshData();
            var newItem = new NewTodoItem
            {
                Description = description
            };

            // act
            var result = await _todoItemsController.PostTodoItem(newItem);

            // assert 
            result.Should().BeOfType<BadRequestObjectResult>();
            var errorMessage = (result as BadRequestObjectResult).Value as string;
            errorMessage.Should().Be(Constants.DesctiptionRequiredMessage);
        }

        [Fact]
        public async Task PostTodoItem_With_Existing_Description_Should_Return_BadRequest()
        {
            // arrange
            RefreshData();
            var newItem = new NewTodoItem
            {
                Description = "Item01"
            };

            // act
            var result = await _todoItemsController.PostTodoItem(newItem);

            // assert 
            result.Should().BeOfType<BadRequestObjectResult>();
            var errorMessage = (result as BadRequestObjectResult).Value as string;
            errorMessage.Should().Be(Constants.DesctiptionExistsMessage);
        }
    }
}
