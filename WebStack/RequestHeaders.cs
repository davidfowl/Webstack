using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace WebStack
{
    public class RequestHeaders : IDictionary<string, string[]>
    {
        private readonly http_headers _headers;
        private static Dictionary<string, int> _headerLookup = GetHeaders();

        public RequestHeaders(http_headers headers)
        {
            _headers = headers;
        }

        public void Add(string key, string[] value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public ICollection<string> Keys
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out string[] value)
        {
            int index;
            if (_headerLookup.TryGetValue(key, out index))
            {
                IntPtr headerPtr = _headers.headers[index];
                if (headerPtr != IntPtr.Zero)
                {
                    var header = (http_header)Marshal.PtrToStructure(headerPtr, typeof(http_header));
                    value = new[] { header.value.GetString() };
                    return true;
                }
            }

            value = null;
            return false;
        }

        public ICollection<string[]> Values
        {
            get { throw new NotImplementedException(); }
        }

        public string[] this[string key]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(KeyValuePair<string, string[]> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, string[]> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, string[]>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(KeyValuePair<string, string[]> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, string[]>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        private static Dictionary<string, int> GetHeaders()
        {
            var knownRequestHeaders = new[] {
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

            var lookup = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < knownRequestHeaders.Length; i++)
            {
                lookup[knownRequestHeaders[i]] = i;
            }

            return lookup;
        }
    }
}
