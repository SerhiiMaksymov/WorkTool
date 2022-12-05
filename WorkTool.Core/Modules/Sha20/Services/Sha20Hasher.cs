namespace WorkTool.Core.Modules.Sha20.Services;

public class Sha20Hasher : IHasher
{
    public byte[] GetHash(byte[] data)
    {
        using var sha1 = SHA1.Create();
        var hashValue = sha1.ComputeHash(data);

        return hashValue;
    }
}
