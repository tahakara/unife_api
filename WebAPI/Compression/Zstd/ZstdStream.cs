using ZstdNet;
using System.IO.Compression;

namespace WebAPI.Compression
{
    public class ZstdStream : Stream
    {
        private readonly Stream _baseStream;
        private readonly CompressionMode _mode;
        private readonly Compressor _compressor;
        private readonly Decompressor _decompressor;
        private bool _disposed = false;
        private readonly MemoryStream _buffer = new MemoryStream();

        public ZstdStream(Stream stream, CompressionMode mode, int level = 3)
        {
            _baseStream = stream ?? throw new ArgumentNullException(nameof(stream));
            _mode = mode;

            if (mode == CompressionMode.Compress)
            {
                _compressor = new Compressor(new CompressionOptions(level));
            }
            else
            {
                _decompressor = new Decompressor();
            }
        }

        public override bool CanRead => _mode == CompressionMode.Decompress && _baseStream.CanRead;
        public override bool CanSeek => false;
        public override bool CanWrite => _mode == CompressionMode.Compress && _baseStream.CanWrite;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public override void Flush() => FlushAsync().GetAwaiter().GetResult();

        public override async Task FlushAsync(CancellationToken cancellationToken = default)
        {
            if (_mode == CompressionMode.Compress && _buffer.Length > 0)
            {
                await WriteBufferAsync();
            }
            await _baseStream.FlushAsync(cancellationToken);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_mode != CompressionMode.Decompress)
                throw new InvalidOperationException("Cannot read from compression stream");

            return _baseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count)
        {
            WriteAsync(buffer, offset, count).GetAwaiter().GetResult();
        }

        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default)
        {
            if (_mode != CompressionMode.Compress)
                throw new InvalidOperationException("Cannot write to decompression stream");

            _buffer.Write(buffer, offset, count);

            // Buffer'ı belirli bir boyuta geldiğinde flush et
            if (_buffer.Length >= 102400) // 100KB 
            {
                await WriteBufferAsync();
            }
        }

        public override async ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (_mode != CompressionMode.Compress)
                throw new InvalidOperationException("Cannot write to decompression stream");

            _buffer.Write(buffer.Span);

            if (_buffer.Length >= 102400)
            {
                await WriteBufferAsync();
            }
        }

        private async Task WriteBufferAsync()
        {
            if (_buffer.Length > 0 && _compressor != null)
            {
                var data = _buffer.ToArray();
                _buffer.SetLength(0);
                _buffer.Position = 0;

                var compressed = _compressor.Wrap(data);
                await _baseStream.WriteAsync(compressed, 0, compressed.Length);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_mode == CompressionMode.Compress && _buffer.Length > 0)
                {
                    WriteBufferAsync().GetAwaiter().GetResult();
                }

                _buffer?.Dispose();
                _compressor?.Dispose();
                _decompressor?.Dispose();
                _baseStream?.Dispose();
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        public override async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                if (_mode == CompressionMode.Compress && _buffer.Length > 0)
                {
                    await WriteBufferAsync();
                }

                _buffer?.Dispose();
                _compressor?.Dispose();
                _decompressor?.Dispose();

                if (_baseStream != null)
                {
                    await _baseStream.DisposeAsync();
                }

                _disposed = true;
            }
            await base.DisposeAsync();
        }
    }
}
