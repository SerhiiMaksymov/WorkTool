namespace WorkTool.Core.Modules.Common.Models;

public readonly struct IndexOfNeedlesResult
{
    public int Index { get; }
    public ReadOnlyMemory<byte> Match { get; }

    public IndexOfNeedlesResult(int index, ReadOnlyMemory<byte> match)
    {
        Index = index;
        Match = match;
    }

    public IndexOfNeedlesResult()
    {
        Index = -1;
    }
}
