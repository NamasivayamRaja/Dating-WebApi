namespace API.Errors
{
    public class ApiException(int statusCode, string errorMessage, string? stackTrace = null)
    {
        public int StatusCode = statusCode;
        public string ErrorMessage = errorMessage;
        public string? Details = stackTrace;
    }
}
