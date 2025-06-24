using System.Net;

namespace CleverCode.Helpers
{
    public class BaseResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public BaseResponse(HttpStatusCode statusCode, string? message = null)
        {
            Message = message ?? string.Empty;
            StatusCode = statusCode;
        }
        public virtual string? GetMessage(HttpStatusCode statusCode)
        {
            return statusCode.ToString();
        }
    }
}
