using System;
using Microsoft.Owin.Hosting;
using Owin;

namespace WebStack.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new StartOptions
            {
                ServerFactory = "WebStack"
            };

            using (WebApp.Start<Startup>(options))
            {
                Console.WriteLine("Running a fake http server");
                Console.ReadKey();
            }
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use(async context =>
            {
                await context.Response.WriteAsync("Hello World");
            });
        }
    }
}
