using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebStack.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var http_server = WebServer.create_server(new Call(Invoke));

            Task.Run(() =>
            {
                WebServer.start_server(http_server);
            });

            Console.ReadKey();

            WebServer.stop_server(http_server);
        }

        private static void Invoke(IntPtr envrionment)
        {
            var request = Marshal.PtrToStructure(envrionment, typeof(owin_request));
        }
    }
}
