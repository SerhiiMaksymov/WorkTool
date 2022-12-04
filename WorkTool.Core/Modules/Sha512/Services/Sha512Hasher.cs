namespace WorkTool.Core.Modules.Sha512.Services;

public class Sha512Hasher : IHasher
{
    public byte[] GetHash(byte[] data)
    {
        using var sha512    = SHA512.Create();
        var       hashValue = sha512.ComputeHash(data);

        return hashValue;
    }
}