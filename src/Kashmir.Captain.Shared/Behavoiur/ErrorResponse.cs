namespace Kashmir.Captain.Shared
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? StackTrace { get; set; }
    }

}