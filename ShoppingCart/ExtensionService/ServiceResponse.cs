namespace ShoppingCart.ExtensionService
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        //public List<string> Errors { get; set; } = new List<string>();
        public List<string> Errors { get; set; } = [];

        public ServiceResponse() { }

        public ServiceResponse(T data)
        {
            Data = data;
        }

        // Factory helpers. Use as: ServiceResponse<T>.Ok(data) or ServiceResponse<T>.Fail(message)
        public static ServiceResponse<T> Ok(T data)
        {
            return new ServiceResponse<T>
            {
                Data = data,
                Success = true
            };
        }

        public static ServiceResponse<T> Fail(string message)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }

        public static ServiceResponse<T> Fail(IEnumerable<string> errors, string message = "")
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors?.ToList() ?? []
            };
        }
    }
}
