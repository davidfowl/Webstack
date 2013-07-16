#include "server.h"
#include <stdlib.h>
#include <string.h>

#define STRLENOF(s) sizeof(s)-1
#define ASSIGN(s, val, length) s.value = (char*)malloc(length+1); s.length = length; strncpy(s.value, val, length); s.value[length]= '\0';
#define SETSTRING(s,val) s.value=val; s.length=STRLENOF(val)

extern WEBSTACKSERVER_EXPORTS http_server* __stdcall create_server(http_callback callback, void* callback_state)
{
    http_server* server = (http_server*)malloc(sizeof(http_server));

    server->callback = callback;
    server->callback_state = callback_state;

    return server;
}

extern WEBSTACKSERVER_EXPORTS int __stdcall start_server(http_server* server, const char* address, short port)
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

extern WEBSTACKSERVER_EXPORTS void __stdcall stop_server(http_server* server)
{
    server->callback = NULL;
    server->callback_state = NULL;

    free(server);
}

extern WEBSTACKSERVER_EXPORTS int __stdcall read_request_body(http_context* context, char* buffer, int length)
{
    // Number of bytes read
    return 0;
}

extern WEBSTACKSERVER_EXPORTS int __stdcall write_response_body(http_context* context, char* buffer, int length)
{
    // 0 - The data was buffered for writing but is currently waiting for drain
    // 1 - Write was successfully flushed to the underlying network
    return 1;
}

extern WEBSTACKSERVER_EXPORTS void __stdcall on_response_drain(stream_drain_callback drain_callback)
{
    // the drain callback is fired when 0 is returned from write and that stream successfully
    // unblocks
    
}