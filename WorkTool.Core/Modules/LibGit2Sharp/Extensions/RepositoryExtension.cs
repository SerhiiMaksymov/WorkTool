namespace WorkTool.Core.Modules.LibGit2Sharp.Extensions;

public static class RepositoryExtension
{
    public static Repository ThrowIfNotHasChanges(
        this Repository repository,
        DirectoryInfo directoryInfo
    )
    {
        var status = repository.RetrieveStatus();
        status.ThrowIfNotHasChanges(directoryInfo);

        return repository;
    }

    public static Repository Stage(this Repository repository, string path)
    {
        Commands.Stage(repository, path);

        return repository;
    }

    public static Signature GetCurrentSignature(this Repository repository)
    {
        var email = repository.Config.Get<string>(ConfigPaths.Email.ToArray());
        var username = repository.Config.Get<string>(ConfigPaths.Username.ToArray());
        var result = new Signature(username.Value, email.Value, DateTimeOffset.Now);

        return result;
    }
}
