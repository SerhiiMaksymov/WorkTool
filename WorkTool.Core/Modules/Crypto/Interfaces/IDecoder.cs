namespace WorkTool.Core.Modules.Crypto.Interfaces;

public interface IDecoder
{
    Task<byte[]> DecryptAsync(byte[] data);
}