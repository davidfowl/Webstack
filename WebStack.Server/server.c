#include <stdlib.h>

#define STRLENOF(s) sizeof(s)-1
#define ASSIGN(s, val, length) s.value = (char*)malloc(length+1); s.length = length; strncpy(s.value, val, length); s.value[length]= '\0';
#define SETSTRING(s,val) s.value=val; s.length=STRLENOF(val)
#define APPENDSTRING(s,val) memcpy((char*)s->value + s->length, val, STRLENOF(val)); s->length+=STRLENOF(val)
#define FREESTRING(s) free(s.value)

typedef struct
{
    char* value;
    int length;

} string;

typedef struct
{
    string name;
    string value;

} header;


typedef struct 
{
    string path;
    string path_base;
    string request_method;
    string request_query_string;
    string request_scheme;

    int header_count;
    header** headers;

} owin_request;

typedef void (__stdcall *callback)(owin_request*, void*);

typedef struct
{
    callback callback;
    void* callback_state;

} http_server;


extern __declspec(dllexport) http_server* __stdcall create_server(callback callback, void* callback_state)
{
    http_server* server = (http_server*)malloc(sizeof(http_server));

    server->callback = callback;
    server->callback_state = callback_state;

    return server;
}

extern __declspec(dllexport) int __stdcall start_server(http_server* server)
{
    while(server->callback != NULL) 
    {
        owin_request* request  = (owin_request*)malloc(sizeof(owin_request));
        SETSTRING(request->path, "/");
        SETSTRING(request->path_base, "");
        SETSTRING(request->request_method, "GET");
        SETSTRING(request->request_query_string, "");
        SETSTRING(request->request_scheme, "http");

        server->callback(request, server->callback_state);

        _sleep(5);
    }

    return 0;   
}

extern __declspec(dllexport) void __stdcall stop_server(http_server* server)
{
    server->callback = NULL;
    server->callback_state = NULL;

    free(server);
}