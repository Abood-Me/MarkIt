namespace MarkItWebAPI.Models
{
    public class TodoApiModel
    {
        public string Text { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}