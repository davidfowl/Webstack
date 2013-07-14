using System;
using System.Runtime.InteropServices;

namespace WebStack
{
    public delegate void Call(IntPtr environment);

    [StructLayout(LayoutKind.Sequential)]
    public struct owin_request
    {
        [MarshalAs(UnmanagedType.Struct)]
        public owin_string path;

        [MarshalAs(UnmanagedType.Struct)]
        public owin_string path_base;

        [MarshalAs(UnmanagedType.Struct)]
        public owin_string request_method;

        [MarshalAs(UnmanagedType.Struct)]
        public owin_string request_query_string;

        [MarshalAs(UnmanagedType.Struct)]
        public owin_string request_scheme;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct owin_string
    {
        public string value;

        public int length;
    }

    public static class WebServer
    {
        [DllImport("WebStack.Server.dll")]
        public static extern IntPtr create_server(Call callback);

        [DllImport("WebStack.Server.dll")]
        public static extern void start_server(IntPtr server);

        [DllImport("WebStack.Server.dll")]
        public static extern void stop_server(IntPtr server);
    }
}
