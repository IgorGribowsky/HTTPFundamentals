using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HTTPFundamentals.Client
{
    internal class Program
    {
        public const string BaseAddress = @"http://localhost:8888/";

        public const string MyNameCookie = "MyName";

        public const string MyNameHeader = "X-MyName";

        public const string MyNameEndpoint = "MyName";

        public const string MyNameByHeaderEndpoint = "MyNameByHeader";

        public const string MyNameByCookieEndpoint = "MyNameByCookie";

        public const string InformationEndpoint = "Information";

        public const string SuccessEndpoint = "Success";

        public const string RedirectionEndpoint = "Redirection";

        public const string ClientErrorEndpoint = "ClientError";

        public const string ServerErrorEndpoint = "ServerError";

        public static readonly HttpClient _client = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.WriteLine("From body:");
            await GetRequestByBody(MyNameEndpoint);
            //await GetRequestFromBody(InformationEndpoint);
            await GetRequestByBody(SuccessEndpoint);
            await GetRequestByBody(RedirectionEndpoint);
            await GetRequestByBody(ClientErrorEndpoint);
            await GetRequestByBody(ServerErrorEndpoint);

            Console.WriteLine("\nFrom header:");
            await GetRequestByHeader(MyNameByHeaderEndpoint, MyNameHeader);

            Console.WriteLine("\nFrom cookie:");
            await GetRequestByCookie(MyNameByCookieEndpoint, MyNameCookie);
            Console.ReadLine();
        }

        private static async Task GetRequestByBody(string endpoint)
        {
            try
            {
                var request = BaseAddress + endpoint;
                Console.WriteLine("\nRequest: " + request);
                var response = await _client.GetAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + responseBody);
                Console.WriteLine("Status code: " + (int)response.StatusCode);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Exception Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        private static async Task GetRequestByHeader(string endpoint, string headerName)
        {
            try
            {
                var request = BaseAddress + endpoint;
                Console.WriteLine("\nRequest: " + request);
                var response = await _client.GetAsync(request);
                var header = response.Headers.GetValues(headerName).ToList().First();
                Console.WriteLine("Header: " + header);
                Console.WriteLine("Status code: " + (int)response.StatusCode);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Exception Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        private static async Task GetRequestByCookie(string endpoint, string cookieName)
        {
            try
            {
                var request = BaseAddress + endpoint;
                var cookies = new CookieContainer();
                var handler = new HttpClientHandler();
                handler.CookieContainer = cookies;

                Console.WriteLine("\nRequest: " + request);

                var client = new HttpClient(handler);
                var response = await client.GetAsync(request);

                var cookie = cookies.GetCookies(new Uri(request)).Cast<Cookie>().FirstOrDefault(cookie => cookie.Name == cookieName);

                if (cookie == null)
                {
                    Console.WriteLine($"Cookie {cookieName} is not found");
                    return;
                }

                Console.WriteLine("Cookie value: " + cookie.Value);
                Console.WriteLine("Status code: " + (int)response.StatusCode);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Exception Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }
}
