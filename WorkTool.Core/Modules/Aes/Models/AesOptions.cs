namespace WorkTool.Core.Modules.Aes.Models;

public class AesOptions
{
    public byte[] Key { get; }
    public byte[] Iv  { get; }

    public AesOptions(byte[] key, byte[] iv)
    {
        Key = key.ThrowIfNull();
        Iv  = iv.ThrowIfNull();
    }
}