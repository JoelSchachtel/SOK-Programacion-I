namespace EstancieroResponse
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse()
        {
            Success = false;
            Message = string.Empty;
            Data = default(T);
        }
    }
}

