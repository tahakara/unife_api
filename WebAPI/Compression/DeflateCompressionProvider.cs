using Microsoft.AspNetCore.ResponseCompression; 
using System.IO.Compression;
using System.IO;

namespace WebAPI.Compression
{
    public class DeflateCompressionProvider : ICompressionProvider
    {
        // Arayüzün gerektirdiği EncodingName özelliğini uygula
        public string EncodingName => "deflate";

        // Arayüzün gerektirdiği SupportsCompressionLevel özelliğini uygula
        public bool SupportsCompressionLevel => true;

        // Arayüzün gerektirdiği SupportsFlush özelliğini uygula
        public bool SupportsFlush => true; 

        // Arayüzün gerektirdiği CreateStream metodu, sıkıştırma seviyesi ile
        public Stream CreateStream(Stream outputStream, CompressionLevel compressionLevel)
        {
            return new DeflateStream(outputStream, compressionLevel, leaveOpen: true);
        }

        // Arayüzün gerektirdiği CreateStream metodu, sıkıştırma seviyesi olmadan (varsayılan)
        public Stream CreateStream(Stream outputStream)
        {
            // Varsayılan bir sıkıştırma seviyesi kullanabiliriz, örneğin Optimal
            return new DeflateStream(outputStream, CompressionLevel.Optimal, leaveOpen: true);
        }
    }

    public class DeflateCompressionProviderOptions
    {
        public CompressionLevel Level { get; set; } = CompressionLevel.Optimal;
    }
}
