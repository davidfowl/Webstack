using System;
using System.IO;

namespace WebStack
{
    public class RequestStream : Stream
    {
        private readonly IntPtr _httpContextPtr;

        public RequestStream(IntPtr httpContextPtr)
        {
            _httpContextPtr = httpContextPtr;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
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

        public unsafe override int Read(byte[] buffer, int offset, int count)
        {
            fixed (byte* bytes = &buffer[offset])
            {
                return WebServer.read_request_body(_httpContextPtr, bytes, count);
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
