namespace WorkTool.Core.Modules.Common.Helpers;

public static class MemoryConstants
{
    public static readonly ReadOnlyMemory<byte> NewLineUtf8  = Environment.NewLine.ToByteArray(Encoding.UTF8);
    public static readonly ReadOnlyMemory<byte> SpaceUtf8    = " ".ToByteArray(Encoding.UTF8);
    public static readonly ReadOnlyMemory<byte> PreambleUtf8 = Encoding.UTF8.Preamble.ToArray();
    public static readonly ReadOnlyMemory<ReadOnlyMemory<byte>> WhiteSpace = new (
        new[]
        {
            PreambleUtf8,
            NewLineUtf8,
            SpaceUtf8
        });
}