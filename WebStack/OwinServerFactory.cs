using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WebStack
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    using CapabilitiesDictionary = IDictionary<string, object>;

    public static class OwinServerFactory
    {
        public static void Initialize(IDictionary<string, object> properties)
        {
            properties[OwinConstants.OwinVersion] = "1.0";


            CapabilitiesDictionary capabilities =
                properties.Get<CapabilitiesDictionary>(OwinConstants.CommonKeys.Capabilities)
                    ?? new Dictionary<string, object>();
            properties[OwinConstants.CommonKeys.Capabilities] = capabilities;

            capabilities["server.Name"] = "WebStack";
        }

        public static IDisposable Create(AppFunc app, IDictionary<string, object> properties)
        {
            var server = new OwinWebStackServer(app, properties);

            server.Start();

            return server;
        }
    }
}
