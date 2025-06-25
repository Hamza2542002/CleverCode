using System.Net;

namespace CleverCode.Helpers
{
    public class ServiceResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public bool Success { get; set; }
        public object? Data { get; set; }
    }
}
