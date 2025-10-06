namespace EstancieroResponse
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public List<string>? Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse()
        {
            Success = false;
            Message = new List<string>();
            Data = default(T);
        }
    }
}

