using HTTPFundamentals.Listener.Http;
using HTTPFundamentals.Listener.Http.Attributes;
using HTTPFundamentals.Listener.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace HTTPFundamentals.Listener
{
    internal class Program
    {
        public const string BaseAddress = @"http://localhost:8888/";

        public const char ForwardSlash = '/';

        public static List<HttpEndpointInfo> Endpoints = new List<HttpEndpointInfo>();

        static void Main(string[] args)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("HttpListener is not supported.");
                return;
            }

            var listener = new HttpListener();
            listener.Prefixes.Add(BaseAddress);

            // Get classes with HttpServiceAttribute
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(HttpServiceAttribute)));
            foreach (var type in types)
            {
                var service = Activator.CreateInstance(type);
                // Get public methods with HttpServiceEndpointAttribute
                var methods = type.GetMethods().Where(method => method.IsPublic && method.IsDefined(typeof(HttpServiceEndpointAttribute)));
                foreach (var method in methods)
                {
                    var attribute = method.GetCustomAttribute(typeof(HttpServiceEndpointAttribute));
                    var endpointAttribute = attribute as HttpServiceEndpointAttribute;
                    var routePath = endpointAttribute.RoutePath.Trim(ForwardSlash);

                    var endpoint = new HttpEndpointInfo
                    {
                        MethodInfo = method,
                        RoutePath = routePath,
                        Service = service,
                    };
                    Endpoints.Add(endpoint);
                    Console.WriteLine($"Endpoind added: {BaseAddress}{endpoint.RoutePath}; Method {endpoint.MethodInfo.Name} from service {type.Name}");
                }
            }
            listener.Start();
            Console.WriteLine("Listening...");
            Task.Run(() => Listen(listener));

            Console.ReadLine();
        }

        private static void Listen(HttpListener listener)
        {
            for (;;)
            {
                var context = listener.GetContext();
                Console.WriteLine("Requested: " + context.Request.Url);

                var routePath = context.Request.RawUrl.Trim(ForwardSlash);

                var endpoint = Endpoints.FirstOrDefault(ep => ep.RoutePath == routePath);
                if (endpoint != null)
                {
                    var method = endpoint.MethodInfo;
                    var service = endpoint.Service;
                    method.Invoke(service, new object[] { context });
                    context.Response.Close();
                }
                else
                {
                    context.Response.StatusCode = 404;
                    context.Response.StatusDescription = "NOT FOUND";
                    context.Response.Close();
                }
            }
        }
    }
}
