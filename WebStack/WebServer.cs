using System;
using System.Runtime.InteropServices;

namespace WebStack
{
    public delegate void http_request_callback(IntPtr http_context, IntPtr callback_state);
    public delegate void stream_drain_callback(IntPtr http_context);

    [StructLayout(LayoutKind.Sequential)]
    public struct http_header
    {
        [MarshalAs(UnmanagedType.Struct)]
        public http_string name;

        [MarshalAs(UnmanagedType.Struct)]
        public http_string value;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct http_context
    {
        [MarshalAs(UnmanagedType.Struct)]
        public http_string request_path;

        [MarshalAs(UnmanagedType.Struct)]
        public http_string request_path_base;

        [MarshalAs(UnmanagedType.Struct)]
        public http_string request_method;

        [MarshalAs(UnmanagedType.Struct)]
        public http_string request_protocol;

        [MarshalAs(UnmanagedType.Struct)]
        public http_string request_query_string;

        [MarshalAs(UnmanagedType.Struct)]
        public http_string request_scheme;

        public IntPtr request_headers;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct http_string
    {
        public IntPtr value;

        public int length;
    }

    public static class WebServer
    {
        [DllImport("WebStack.Server.dll")]
        public static extern IntPtr create_server(http_request_callback callback, IntPtr callback_state);

        [DllImport("WebStack.Server.dll")]
        public static extern void start_server(IntPtr http_server, string address, short port);

        [DllImport("WebStack.Server.dll")]
        public static extern void stop_server(IntPtr http_server);

        [DllImport("WebStack.Server.dll")]
        public unsafe static extern int read_request_body(IntPtr http_context, byte* buffer, int length);

        [DllImport("WebStack.Server.dll")]
        public unsafe static extern int write_response_body(IntPtr http_context, byte* buffer, int length);

        [DllImport("WebStack.Server.dll")]
        public unsafe static extern void on_response_drain(stream_drain_callback drain_callback);
    }


    public static class WebServerExtensions
    {
        public static string GetString(this http_string @string)
        {
            return Marshal.PtrToStringAnsi(@string.value, @string.length);
        }
    }
}
