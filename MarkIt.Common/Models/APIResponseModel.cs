namespace MarkIt.Common.Models
{
    public class APIResponseModel<T>
    {
        public bool Succeeded { get; set; }
        public ICollection<string>? Errors { get; set; }

        public T? Response { get; set; }
    }
}