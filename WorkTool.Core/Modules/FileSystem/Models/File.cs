namespace WorkTool.Core.Modules.FileSystem.Models;

public readonly record struct File(Path Path, FileName FileName, ulong Size)
{
    public static implicit operator File(FileInfo file)
    {
        return new(file.FullName, file, (ulong)file.Length);
    }
}
