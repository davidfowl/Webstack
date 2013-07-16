#include <stdlib.h>
#include <string.h>

#define STRLENOF(s) sizeof(s)-1
#define ASSIGN(s, val, length) s.value = (char*)malloc(length+1); s.length = length; strncpy(s.value, val, length); s.value[length]= '\0';
#define SETSTRING(s,val) s.value=val; s.length=STRLENOF(val)

static char* known_request_headers[] = {
    "cache-control",
    "connection",
    "date",
    "keep-alive",
    "pragma",
    "trailer",
    "transfer-encoding",
    "upgrade",
    "via",
    "warning",
    "alive",
    "content-length",
    "content-type",
    "content-encoding",
    "content-language",
    "content-location",
    "content-md5",
    "content-range",
    "expires",
    "last-modified",
    "accept",
    "accept-charset",
    "accept-encoding",
    "accept-language",
    "authorization",
    "cookie",
    "expect",
    "from",
    "host",
    "if-match",
    "if-modified-since",
    "if-none-match",
    "if-range",
    "if-unmodified-since",
    "max-forwards",
    "proxy-authorization",
    "referer",
    "range",
    "te",
    "translate",
    "user-agent"
};

typedef struct
{
    char* value;
    int length;

} http_string;

typedef struct
{
    http_string name;
    http_string value;

} http_header;

typedef struct
{
    int header_count;
    http_header* headers[30];

} http_headers;

typedef struct 
{
    http_string request_path;
    http_string request_path_base;
    http_string request_method;
    http_string request_protocol;
    http_string request_query_string;
    http_string request_scheme;

    http_headers request_headers;

} http_context;

typedef void (__stdcall *http_callback)(http_context*, void*);
typedef void (__stdcall *stream_drain_callback)(http_context*);

typedef struct
{
    http_callback callback;
    void* callback_state;

} http_server;

extern __declspec(dllexport) int __stdcall read_request_body(http_context* context, char* buffer, int length)
{
    // Number of bytes read
    return 0;
}

extern __declspec(dllexport) int __stdcall write_response_body(http_context* context, char* buffer, int length)
{
    // 0 - The data was buffered for writing but is currently waiting for drain
    // 1 - Write was successfully flushed to the underlying network
    return 1;
}

extern __declspec(dllexport) void __stdcall on_response_drain(stream_drain_callback drain_callback)
{
    // the drain callback is fired when 0 is returned from write and that stream successfully
    // unblocks
    
}

extern __declspec(dllexport) http_server* __stdcall create_server(http_callback callback, void* callback_state)
{
    http_server* server = (http_server*)malloc(sizeof(http_server));

    server->callback = callback;
    server->callback_state = callback_state;

    return server;
}

extern __declspec(dllexport) int __stdcall start_server(http_server* server, const char* address, short port)
{
    http_header* header;
    http_context* context;

    while(server->callback != NULL) 
    {
        context = (http_context*)malloc(sizeof(http_context));
        memset(context->request_headers.headers, 0, sizeof(context->request_headers.headers));

        SETSTRING(context->request_path, "/");
        SETSTRING(context->request_path_base, "");
        SETSTRING(context->request_method, "GET");
        SETSTRING(context->request_protocol, "HTTP/1.1");
        SETSTRING(context->request_query_string, "");
        SETSTRING(context->request_scheme, "http");

        context->request_headers.header_count = 1;
        context->request_headers.headers[0] = (http_header*)malloc(sizeof(http_header));
        header = context->request_headers.headers[0];

        SETSTRING(header->name, "Host");
        SETSTRING(header->value, "http://localhost");

        server->callback(context, server->callback_state);

        _sleep(1000);
    }

    return 0;   
}

extern __declspec(dllexport) void __stdcall stop_server(http_server* server)
{
    server->callback = NULL;
    server->callback_state = NULL;

    free(server);
}