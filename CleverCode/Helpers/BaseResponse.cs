using System.Net;

namespace CleverCode.Helpers
{
    public class BaseResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public BaseResponse(HttpStatusCode statusCode,object? data ,string? message = null)
        {
            StatusCode = statusCode;

            Data = data;

            Message = message ?? GetMessage(statusCode);

        }
        public virtual string? GetMessage(HttpStatusCode statusCode)
        {
            return statusCode.ToString();
        }
    }
}
