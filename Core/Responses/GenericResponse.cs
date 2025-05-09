namespace Core.Responses
{
    public class GenericResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new List<string>();
        public object Data { get; set; } = new object();
        public GenericResponse()
        {
            Success = false;
        }
    }
}
