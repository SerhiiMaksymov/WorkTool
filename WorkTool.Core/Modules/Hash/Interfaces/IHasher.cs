namespace WorkTool.Core.Modules.Hash.Interfaces;

public interface IHasher
{
    byte[] GetHash(byte[] data);
}
