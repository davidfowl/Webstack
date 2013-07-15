using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStack
{
    class ResponseStream : Stream
    {
        private readonly IntPtr _httpContextPtr;

        public ResponseStream(IntPtr httpContextPtr)
        {
            _httpContextPtr = httpContextPtr;
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {

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

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public unsafe override void Write(byte[] buffer, int offset, int count)
        {
            fixed (byte* bytes = &buffer[offset])
            {
                WebServer.write_response_body(_httpContextPtr, bytes, count);
            }
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, System.Threading.CancellationToken cancellationToken)
        {
            // TODO: Implement
            return base.WriteAsync(buffer, offset, count, cancellationToken);
        }
    }
}
