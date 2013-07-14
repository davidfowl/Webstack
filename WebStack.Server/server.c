#include <stdlib.h>

#define STRLENOF(s) sizeof(s)-1
#define SETSTRING(s,val) s.value=val; s.length=STRLENOF(val)
#define APPENDSTRING(s,val) memcpy((char*)s->value + s->length, val, STRLENOF(val)); s->length+=STRLENOF(val)

typedef struct 
{
    char* value;
    int length;

} string;

typedef struct 
{
    string path;
    string path_base;
    string request_method;
    string request_query_string;
    string request_scheme;

} owin_request;

typedef void (__stdcall *callback)(owin_request*);

typedef struct
{
    callback handler;

} http_server;

extern __declspec(dllexport) http_server* __stdcall create_server(callback call)
{
    http_server* server = (http_server*)malloc(sizeof(http_server));

    server->handler = call;	

    return server;
}

extern __declspec(dllexport) void __stdcall start_server(http_server* server)
{
    owin_request* r = (owin_request*)malloc(sizeof(owin_request));
    SETSTRING(r->path, "/signalr");
    SETSTRING(r->path_base, "");
    SETSTRING(r->request_method, "GET");
    SETSTRING(r->request_query_string, "");
    SETSTRING(r->request_scheme, "http");

    server->handler(r);

}

extern __declspec(dllexport) void __stdcall stop_server(http_server* server)
{
    server->handler = NULL;

    free(server);
}