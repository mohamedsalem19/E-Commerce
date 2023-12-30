namespace ECommerce.API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefultMessageForStatusCode(statusCode);
        }

        private string? GetDefultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request you have made ",
                401 => "you are not Authorized ",
                404 => "Resource was not found",
                500 => "server error",
                _ => null
            };
        }
    }
}
