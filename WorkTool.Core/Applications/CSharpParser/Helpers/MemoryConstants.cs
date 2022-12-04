namespace WorkTool.Core.Applications.CSharpParser.Helpers;

public static class MemoryConstants
{
    public static readonly ReadOnlyMemory<byte> UsingUtf8     = "using".ToByteArray(Encoding.UTF8);
    public static readonly ReadOnlyMemory<byte> NamespaceUtf8 = "namespace".ToByteArray(Encoding.UTF8);
}