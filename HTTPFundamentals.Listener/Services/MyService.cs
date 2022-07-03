using HTTPFundamentals.Listener.Http;
using HTTPFundamentals.Listener.Http.Attributes;
using System.Net;

namespace HTTPFundamentals.Listener.Services
{
    [HttpService]
    public class MyService : HttpServiceBase
    {
        public const string MyName = "Igor Gribowsky";

        public const string SuccessMessage = "Successful request";

        public const string RedirectionMessage = @"Resourse has been temporarily moved to http://localhost:8888/MyName/";

        public const string ClientErrorMessage = "Request forbidden";

        public const string ServerErrorMessage = "Something went wrong on the server";

        public const string MyNameHeader = "X-MyName";

        public const string MyNameCookie = "MyName";

        [HttpServiceEndpoint("MyName/")]
        public void GetMyName(HttpListenerContext context)
        {
            WriteStringContentToResponse(context, MyName);
        }

        [HttpServiceEndpoint("MyNameByHeader/")]
        public void GetMyNameByHeader(HttpListenerContext context)
        {
            context.Response.Headers.Add(MyNameHeader, MyName);
        }

        [HttpServiceEndpoint("MyNameByCookie/")]
        public void GetMyNameByCookie(HttpListenerContext context)
        {
            context.Response.SetCookie( new Cookie(MyNameCookie, MyName));
        }

        [HttpServiceEndpoint("Information")]
        public void GetInformation(HttpListenerContext context)
        {
            context.Response.StatusCode = 101;
        }

        [HttpServiceEndpoint("Success")]
        public void GetSuccess(HttpListenerContext context)
        {
            context.Response.StatusCode = 200;
            WriteStringContentToResponse(context, SuccessMessage);
        }

        [HttpServiceEndpoint("Redirection")]
        public void GetRedirection(HttpListenerContext context)
        {
            context.Response.StatusCode = 302;  
            WriteStringContentToResponse(context, RedirectionMessage);
        }

        [HttpServiceEndpoint("ClientError")]
        public void GetClientError(HttpListenerContext context)
        {
            context.Response.StatusCode = 403;
            WriteStringContentToResponse(context, ClientErrorMessage);
        }

        [HttpServiceEndpoint("ServerError")]
        public void GetServerError(HttpListenerContext context)
        {
            context.Response.StatusCode = 500;
            WriteStringContentToResponse(context, ServerErrorMessage);
        }
    }
}
