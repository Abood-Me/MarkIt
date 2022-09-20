namespace MarkItWebAPI.Data
{
    public class Todo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }

        public bool IsCompleted { get; set; }
    }
}
