namespace WorkTool.Core.Modules.FileSystem.Models;

public readonly record struct Path(ReadOnlyMemory<char> Value, ReadOnlyMemory<PathSegment> Segments)
{
    public static implicit operator Path(string path)
    {
        var segments = path.Split(SystemPath.DirectorySeparatorChar)
            .Select(x => (PathSegment)x)
            .ToArray();

        return new Path(path.AsMemory(), segments);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
