namespace TodoList.Api.Models
{
    public class NewTodoItem
    {
        public string Description { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}
