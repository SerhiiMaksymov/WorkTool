namespace WorkTool.Core.Modules.Git.Exceptions;

public class GitNotHaveChangesException : Exception
{
    public GitNotHaveChangesException(DirectoryInfo repository)
        : base($"Repository {repository} not has any changes.")
    {
        Repository = repository;
    }

    public DirectoryInfo Repository { get; }
}
