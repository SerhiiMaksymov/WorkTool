namespace WorkTool.Core.Modules.FileSystem.Models;

public readonly record struct Path(ReadOnlyMemory<PathSegment> Segments)
{
    public static implicit operator Path(string path)
    {
        var segments = path.Split(SystemPath.PathSeparator).Select(x => (PathSegment)x).ToArray();

        return new Path(segments);
    }
}
