namespace WorkTool.Core.Modules.FileSystem.Models;

public readonly record struct FileName(ReadOnlyMemory<char> Name, ReadOnlyMemory<char> Extension)
{
    public static implicit operator FileName(FileInfo file)
    {
        var name = SystemPath.GetFileNameWithoutExtension(file.FullName).ToArray();
        var extension = SystemPath.GetExtension(file.FullName).Replace(".", string.Empty).ToArray();

        return new(name, extension);
    }

    public override string ToString()
    {
        if (Extension.IsEmpty)
        {
            return Name.ToString();
        }
        
        return $"{Name}.{Extension}";
    }
}
