namespace WorkTool.Core.Modules.Aes.Services;

public class AesCrypto : ICrypto
{
    public AesOptions Options { get; }

    public AesCrypto(AesOptions options)
    {
        Options = options.ThrowIfNull();
    }

    public async Task<byte[]> DecryptAsync(byte[] data)
    {
        using var aesAlg = System.Security.Cryptography.Aes.Create();
        aesAlg.Key = Options.Key;
        aesAlg.IV  = Options.Iv;
        var             decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
        await using var msDecrypt = new MemoryStream(data);
        await using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

        return msDecrypt.ToArray();
    }

    public async Task<byte[]> EncryptAsync(byte[] data)
    {
        using var aesAlg = System.Security.Cryptography.Aes.Create();
        aesAlg.Key = Options.Key;
        aesAlg.IV  = Options.Iv;
        var             encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
        await using var msEncrypt = new MemoryStream(data);
        await using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

        return msEncrypt.ToArray();
    }
}