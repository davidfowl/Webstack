using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WebStack
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class OwinWebStackServer : IDisposable
    {
        private readonly IntPtr _httpServer;
        private readonly AppFunc _appFunc;
        private readonly GCHandle _callbackHandle;

        private string _host;
        private short _port;

        private delegate Task OwinAppDelegate(IDictionary<string, object> environment);

        public OwinWebStackServer(AppFunc appFunc, IDictionary<string, object> properties)
        {
            _appFunc = appFunc;
            
            // Setup the host and port
            InitalizeHostAndPort(properties);

            // Wrap the generic delegate with a typed delegate to appease the CLR
            OwinAppDelegate appFuncWrapper = env =>
            {
                return appFunc(env);
            };

            // Allocate the callback for the server to call
            var callback = new http_request_callback(OnHttpRequest);

            // Create a GCHandle for the callback so it doesn't get GCed
            _callbackHandle = GCHandle.Alloc(callback);

            // Create the webserver
            _httpServer = WebServer.create_server(callback, IntPtr.Zero);
        }

        private void InitalizeHostAndPort(IDictionary<string, object> properties)
        {
            var addresses = properties.Get<IList<IDictionary<string, object>>>("host.Addresses");
            var address = addresses[0];

            _host = address.Get<string>("host") ?? "localhost";
            _port = short.Parse(address.Get<string>("port") ?? "5000");
        }

        public void Start()
        {
            // TODO: Get rid of this eventually
            Task.Run(() =>
            {
                WebServer.start_server(_httpServer, _host, _port);
            });
        }

        private void OnHttpRequest(IntPtr http_context, IntPtr callback_state)
        {
            var env = new OwinEnvironment(http_context);

            _appFunc.Invoke(env)
                .ContinueWith(task =>
                {
                    // TODO: Make this callback async
                });
        }

        public void Dispose()
        {
            _callbackHandle.Free();
        }
    }
}
