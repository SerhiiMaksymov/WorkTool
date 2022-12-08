namespace WorkTool.Core.Modules.LibGit2Sharp.Helpers;

public static class ConfigPaths
{
    public static readonly ReadOnlyMemory<string> Username = new[] { "user", "name" };
    public static readonly ReadOnlyMemory<string> Email = new[] { "user", "email" };
}
