namespace WorkTool.Core.Modules.Sha256.Services;

public class Sha256Hasher : IHasher
{
    public byte[] GetHash(byte[] data)
    {
        using var sha256 = SHA256.Create();
        var hashValue = sha256.ComputeHash(data);

        return hashValue;
    }
}
