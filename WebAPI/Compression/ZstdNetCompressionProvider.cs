using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using WebAPI.Compression.Zstd;
using Microsoft.Extensions.Options;

namespace WebAPI.Compression
{
    public class ZstdCompressionProvider : ICompressionProvider
    {
        private readonly ZstdCompressionProviderOptions _options;
        public ZstdCompressionProvider(IOptions<ZstdCompressionProviderOptions> options)
        {
            _options = options.Value;
        }

        public string EncodingName => "zstd";
        public bool SupportsFlush => true;

        public Stream CreateStream(Stream outputStream)
        {
            return new ZstdStream(outputStream, CompressionMode.Compress, _options.Level);
        }
    }

    public class ZstdCompressionProviderOptions
    {
        public int Level { get; set; } = 3;
    }
}