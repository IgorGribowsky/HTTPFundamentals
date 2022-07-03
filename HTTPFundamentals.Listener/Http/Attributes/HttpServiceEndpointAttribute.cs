using System;

namespace HTTPFundamentals.Listener.Http.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpServiceEndpointAttribute : Attribute
    {
        public HttpServiceEndpointAttribute(string routePath)
        {
            RoutePath = routePath;
        }

        public string RoutePath { get; set; }
    }
}
