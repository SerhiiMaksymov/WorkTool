namespace WorkTool.Core.Modules.FileSystem.Models;

public readonly record struct PathSegment(ReadOnlyMemory<char> Value)
{
    public static implicit operator PathSegment(string segment)
    {
        return new(segment.ToArray());
    }
}
