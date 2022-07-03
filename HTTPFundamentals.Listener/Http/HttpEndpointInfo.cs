using System.Reflection;

namespace HTTPFundamentals.Listener.Http
{
    public class HttpEndpointInfo
    {
        public string RoutePath { get; set; }

        public MethodInfo MethodInfo { get; set; }

        public object Service { get; set; }

    }
}
