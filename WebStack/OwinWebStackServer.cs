using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WebStack
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    class OwinWebStackServer : IDisposable
    {
        private readonly AppFunc _app;
        private readonly http_request_callback _callback;
        private readonly IntPtr _httpServer;

        private delegate Task OwinAppDelegate(IDictionary<string, object> environment);

        public OwinWebStackServer(AppFunc app, IDictionary<string, object> properties)
        {
            _app = app;
            _callback = new http_request_callback(OnHttpRequest);

            OwinAppDelegate appFuncWrapper = env =>
            {
                return _app(env);
            };

            IntPtr appFuncThunk = Marshal.GetFunctionPointerForDelegate(appFuncWrapper);

            _httpServer = WebServer.create_server(_callback, appFuncThunk);
        }

        public void Start()
        { 
            Task.Run(() =>
            {
                // TODO: Flow addresses
                WebServer.start_server(_httpServer);
            });

        }

        private static void OnHttpRequest(IntPtr http_context, IntPtr callback_state)
        {
            var appFunc = (OwinAppDelegate)Marshal.GetDelegateForFunctionPointer(callback_state, typeof(OwinAppDelegate));

            appFunc.Invoke(new OwinEnvironment(http_context))
                   .ContinueWith(task =>
                   {

                   });
        }

        public void Dispose()
        {
            
        }
    }
}
