using System.Net;

namespace CleverCode.Helpers.Error_Response
{
    public class ErrorResponse : BaseResponse
    {
        public ErrorResponse(HttpStatusCode statusCode, string? message = null) 
            : base(statusCode, message)
        {
        }
        public override string? GetMessage(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.NotFound => "Resource Not Found",
                HttpStatusCode.BadRequest => "Bad Request",
                HttpStatusCode.BadGateway => "Bad Gatway",
                HttpStatusCode.Unauthorized => "Unothorized to acces this Resource",
                HttpStatusCode.InternalServerError => "Server Error",
                _ => string.Empty,
            };
        }
    }
}
