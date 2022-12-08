namespace WorkTool.Core.Modules.LibGit2Sharp.Extensions;

public static class RepositoryStatusExtension
{
    public static RepositoryStatus ThrowIfNotHasChanges(
        this RepositoryStatus status,
        DirectoryInfo directoryInfo
    )
    {
        if (status.IsDirty)
        {
            return status;
        }

        throw new GitNotHaveChangesException(directoryInfo);
    }
}
