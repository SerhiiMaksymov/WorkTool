namespace _build;

public static class RepositoryStatusExtension
{
    public static RepositoryStatus LogStatus(this RepositoryStatus repositoryStatus)
    {
        StatusEntriesLog(repositoryStatus, s => s.Ignored);
        StatusEntriesLog(repositoryStatus, s => s.Added);
        StatusEntriesLog(repositoryStatus, s => s.Staged);
        StatusEntriesLog(repositoryStatus, s => s.Removed);
        StatusEntriesLog(repositoryStatus, s => s.Missing);
        StatusEntriesLog(repositoryStatus, s => s.Modified);
        StatusEntriesLog(repositoryStatus, s => s.Untracked);
        StatusEntriesLog(repositoryStatus, s => s.RenamedInIndex);
        StatusEntriesLog(repositoryStatus, s => s.RenamedInWorkDir);
        StatusEntriesLog(repositoryStatus, s => s.Unaltered);

        return repositoryStatus;
    }

    public static void StatusEntriesLog(
        RepositoryStatus status,
        Expression<Func<RepositoryStatus, IEnumerable<StatusEntry>>> func
    )
    {
        var name = ((MemberExpression)func.Body).Member.Name;
        var entries = func.Compile().Invoke(status);

        if (!entries.Any())
        {
            return;
        }

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"{name}:");
        var index = 0;

        foreach (var entry in entries)
        {
            index++;
            stringBuilder.AppendLine($"{index}.    {entry.FilePath}");
        }

        Log.Information(stringBuilder.ToString());
    }
}
