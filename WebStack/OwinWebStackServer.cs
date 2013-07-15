using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WebStack
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class OwinWebStackServer : IDisposable
    {
        private readonly AppFunc _app;
        private readonly http_request_callback _callback;
        private readonly IntPtr _httpServer;
        private readonly string _host;
        private readonly short _port;

        private delegate Task OwinAppDelegate(IDictionary<string, object> environment);

        public OwinWebStackServer(AppFunc app, IDictionary<string, object> properties)
        {
            _app = app;
            _callback = new http_request_callback(OnHttpRequest);
            var address = ((IList<IDictionary<string, object>>)properties["host.Addresses"])[0];

            // We only support http for now
            // string scheme = address.Get<string>("scheme") ?? Uri.UriSchemeHttp;
            _host = address.Get<string>("host") ?? "localhost";
            string port = address.Get<string>("port") ?? "5000";
            _port = short.Parse(port);

            OwinAppDelegate appFuncWrapper = env =>
            {
                return _app(env);
            };

            IntPtr appFuncThunk = Marshal.GetFunctionPointerForDelegate(appFuncWrapper);

            _httpServer = WebServer.create_server(_callback, appFuncThunk);
        }

        public void Start()
        {
            // TODO: Get rid of this eventually
            Task.Run(() =>
            {
                WebServer.start_server(_httpServer, _host, _port);
            });
        }

        private static void OnHttpRequest(IntPtr http_context, IntPtr callback_state)
        {
            // REVIEW: If this is too slow then we can consider a static dictionary
            // mapping the http_server to the app func
            var appFunc = (OwinAppDelegate)Marshal.GetDelegateForFunctionPointer(callback_state, typeof(OwinAppDelegate));

            var env = new OwinEnvironment(http_context);

            appFunc.Invoke(env)
                   .ContinueWith(task =>
                   {
                       // TODO: Make this async
                   });
        }

        public void Dispose()
        {

        }
    }
}
