namespace WorkTool.Core.Modules.Crypto.Interfaces;

public interface IEncoder
{
    Task<byte[]> EncryptAsync(byte[] data);
}
