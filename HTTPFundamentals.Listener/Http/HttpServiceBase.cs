using System.Net;
using System.Text;

namespace HTTPFundamentals.Listener.Http
{
    public class HttpServiceBase
    {
        protected void WriteStringContentToResponse(HttpListenerContext context, string content)
        {
            var response = context.Response;
            var responseString = content;
            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
    }
}
