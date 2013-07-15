using System;
using System.Runtime.InteropServices;

namespace WebStack
{
    public delegate void Complete(IntPtr state);

    public delegate void Call(IntPtr env, IntPtr callback_state);

    [StructLayout(LayoutKind.Sequential)]
    public struct header
    {
        [MarshalAs(UnmanagedType.Struct)]
        public owin_string name;

        [MarshalAs(UnmanagedType.Struct)]
        public owin_string value;
    }

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

        public int header_length;

        public IntPtr headers;
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
        public static extern IntPtr create_server(Call callback, IntPtr state);

        [DllImport("WebStack.Server.dll")]
        public static extern void start_server(IntPtr server);

        [DllImport("WebStack.Server.dll")]
        public static extern void stop_server(IntPtr server);
    }
}
