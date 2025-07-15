using WebAPI.Compression.Zstd;
using ZstdNet;

namespace WebAPI.Compression.Zstd
{
    public class ZstdService : IZstdService
    {
        public byte[] Compress(byte[] data)
        {
            using var compressor = new Compressor();
            return compressor.Wrap(data);
        }

        public byte[] Decompress(byte[] compressedData)
        {
            using var decompressor = new Decompressor();
            return decompressor.Unwrap(compressedData);
        }

        public string CompressString(string text)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            var compressed = Compress(bytes);
            return Convert.ToBase64String(compressed);
        }

        public string DecompressString(byte[] compressedData)
        {
            var decompressed = Decompress(compressedData);
            return System.Text.Encoding.UTF8.GetString(decompressed);
        }
    }
}