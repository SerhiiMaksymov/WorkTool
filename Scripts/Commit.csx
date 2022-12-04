#r"nuget:LibGit2Sharp, 0.27.0-preview-0182"

using LibGit2Sharp;

using System;
using System.Linq.Expressions;

var repositoryDirectory = GetRepositoryDirectory(Args);
var repository = new Repository(repositoryDirectory.FullName);
var statusOptions = new StatusOptions();
var status = repository.RetrieveStatus(statusOptions);
var stringInfo = GetStringInfo(repository);
StatusToConsole(status);
Console.WriteLine(stringInfo);
Console.WriteLine("Added all?(yes/no)");
WaitForYesOrNo();
Commands.Stage(repository, "*");
status = repository.RetrieveStatus(statusOptions);
StatusToConsole(status);
Console.WriteLine("Want to comment?");
WaitForYesOrNo();
Console.WriteLine("Input commnet message:");
var message = Console.ReadLine();
var author = new Signature(GetUsername(repository.Config), GetEmail(repository.Config), DateTime.Now);
repository.Commit(message, author, author);

public static void WaitForYesOrNo()
{
    while (true)
    {
        var line = Console.ReadLine();

        if (line.Equals("no", StringComparison.InvariantCultureIgnoreCase))
        {
            Environment.Exit(Environment.ExitCode);
        }

        if (line.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
        {
            break;
        }
    }
}

public static void StatusToConsole(RepositoryStatus status)
{
    StatusEntriesToConsole(status, s => s.Ignored, ConsoleColor.DarkRed);
    StatusEntriesToConsole(status, s => s.Added, ConsoleColor.Blue);
    StatusEntriesToConsole(status, s => s.Staged, ConsoleColor.Cyan);
    StatusEntriesToConsole(status, s => s.Removed, ConsoleColor.DarkBlue);
    StatusEntriesToConsole(status, s => s.Missing, ConsoleColor.DarkCyan);
    StatusEntriesToConsole(status, s => s.Modified, ConsoleColor.DarkGray);
    StatusEntriesToConsole(status, s => s.Untracked, ConsoleColor.DarkGreen);
    StatusEntriesToConsole(status, s => s.RenamedInIndex, ConsoleColor.DarkYellow);
    StatusEntriesToConsole(status, s => s.RenamedInWorkDir, ConsoleColor.Gray);
    StatusEntriesToConsole(status, s => s.Unaltered, ConsoleColor.Green);
    Console.ForegroundColor = ConsoleColor.White;
}

public static void StatusEntriesToConsole(RepositoryStatus status, Expression<Func<RepositoryStatus, IEnumerable<StatusEntry>>> func, ConsoleColor foreground)
{
    Console.ForegroundColor = foreground;
    StatusEntriesToConsole(status, func);
}

public static void StatusEntriesToConsole(RepositoryStatus status, Expression<Func<RepositoryStatus, IEnumerable<StatusEntry>>> func)
{
    var name = ((MemberExpression)func.Body).Member.Name;
    var entries = func.Compile().Invoke(status);

    if (!entries.Any())
    {
        return;
    }

    Console.WriteLine($"{name}:");
    var index = 0;

    foreach (var entry in entries)
    {
        index++;
        Console.WriteLine($"{index}.    {entry.FilePath}");
    }
}

public static string GetStringInfo(Repository repository)
{
    return $$"""
Branch:   {{repository.Head.FriendlyName}}
Username: {{GetUsername(repository.Config)}}
Login:    {{GetLogin(repository.Config)}}
Email:    {{GetEmail(repository.Config)}}
""";
}

public static string GetConfigurationValue(Configuration config, string[] path)
{
    return config.Get<string>(path).Value;
}

public static string GetEmail(Configuration config)
{
    return GetConfigurationValue(config, new[] { "user", "email" });
}

public static string GetLogin(Configuration config)
{
    return GetConfigurationValue(config, new[] { "user", "login" });
}

public static string GetUsername(Configuration config)
{
    return GetConfigurationValue(config, new[] { "user", "name" });
}

public static bool IsDirectoryPath(string path)
{
    if (string.IsNullOrWhiteSpace(path))
    {
        return false;
    }

    try
    {
        new DirectoryInfo(path);

        return true;
    }
    catch
    {
        return false;
    }
}

public static DirectoryInfo GetRepositoryDirectory(IList<string> args)
{
    if (args.Count == 0)
    {
        return new DirectoryInfo(Directory.GetCurrentDirectory());
    }

    if (IsDirectoryPath(args[0]))
    {
        return new DirectoryInfo(Directory.GetCurrentDirectory());
    }

    var directory = new DirectoryInfo(args[0]);

    if (!directory.Exists)
    {
        return new DirectoryInfo(Directory.GetCurrentDirectory());
    }

    return directory;
}
