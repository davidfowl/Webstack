#ifdef WEBSTACKSERVER_EXPORTS
#define WEBSTACKSERVER_EXPORTS __declspec(dllexport)
#else
#define WEBSTACKSERVER_EXPORTS __declspec(dllimport)
#endif

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


extern WEBSTACKSERVER_EXPORTS http_server* __stdcall create_server(http_callback callback, void* callback_state);
extern WEBSTACKSERVER_EXPORTS int __stdcall start_server(http_server* server, const char* address, short port);
extern WEBSTACKSERVER_EXPORTS void __stdcall stop_server(http_server* server);

extern WEBSTACKSERVER_EXPORTS int __stdcall read_request_body(http_context* context, char* buffer, int length);
extern WEBSTACKSERVER_EXPORTS int __stdcall write_response_body(http_context* context, char* buffer, int length);
extern WEBSTACKSERVER_EXPORTS void __stdcall on_response_drain(stream_drain_callback drain_callback);