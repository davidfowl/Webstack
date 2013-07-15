using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WebStack
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public static class OwinServerFactory
    {
        private delegate Task OwinAppDelegate(IDictionary<string, object> environment);

        public static void Initialize(IDictionary<string, object> properties)
        {

        }

        public static IDisposable Create(AppFunc app, IDictionary<string, object> properties)
        {
            OwinAppDelegate appFuncWrapper = env =>
            {
                return app(env);
            };

            IntPtr appFunc = Marshal.GetFunctionPointerForDelegate(appFuncWrapper);

            IntPtr http_server = WebServer.create_server(new Call(Invoke), appFunc);

            Task.Run(() =>
            {
                // TODO: Flow addresses
                WebServer.start_server(http_server);
            });

            return new DisosableAction(() =>
            {
                WebServer.stop_server(http_server);
            });
        }

        private static void Invoke(IntPtr env, IntPtr callback_state)
        {
            var appFunc = (OwinAppDelegate)Marshal.GetDelegateForFunctionPointer(callback_state, typeof(OwinAppDelegate));

            appFunc.Invoke(new OwinEnvironment(env))
                   .ContinueWith(task =>
                   {
                       
                   });
        }

        private class DisosableAction : IDisposable
        {
            private readonly Action _action;
            public DisosableAction(Action action)
            {
                _action = action;
            }

            public void Dispose()
            {
                _action();
            }
        }
    }
}
