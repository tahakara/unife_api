namespace WebAPI.Compression.Zstd
{
    public interface IZstdService
    {
        byte[] Compress(byte[] data);
        byte[] Decompress(byte[] compressedData);
        string CompressString(string text);
        string DecompressString(byte[] compressedData);
    }
}