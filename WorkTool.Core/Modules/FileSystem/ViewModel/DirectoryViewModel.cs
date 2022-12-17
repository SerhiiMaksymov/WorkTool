namespace WorkTool.Core.Modules.FileSystem.ViewModel;

public class DirectoryViewModel : NotifyBase
{
    private QuantitiesInformation size;
    private DirectoryInfo? directory;

    public DirectoryViewModel()
    {
        Directories = new();
    }

    public string? Name => directory?.Name;
    public AvaloniaList<DirectoryViewModel> Directories { get; }

    public DirectoryInfo? Directory
    {
        get => directory;
        set => this.RaiseAndSetIfChanged(ref directory, value);
    }

    public QuantitiesInformation Size
    {
        get => size;
        set => this.RaiseAndSetIfChanged(ref size, value);
    }

    public async IAsyncEnumerator<QuantitiesInformation> RunAsync(CancellationToken token)
    {
        Directory = Directory.ThrowIfNull();
        var files = await GetFilesAsync(Directory, token);
        token.ThrowIfCancellationRequested();

        foreach (var file in files)
        {
            var fileSize = await GetSizeAsync(file, token);
            token.ThrowIfCancellationRequested();
            Size += fileSize;

            yield return fileSize;
        }

        var directories = await GetDirectoriesAsync(Directory, token);
        token.ThrowIfCancellationRequested();
        var tasks = new List<IAsyncEnumerator<QuantitiesInformation>>();

        foreach (var dir in directories)
        {
            var viewModel = new DirectoryViewModel { Directory = dir };

            Directories.Add(viewModel);
            tasks.Add(viewModel.RunAsync(token));
        }

        if (directories.IsEmpty())
        {
            yield break;
        }

        var removed = new List<IAsyncEnumerator<QuantitiesInformation>>();

        while (true)
        {
            foreach (var task in tasks)
            {
                if (await task.MoveNextAsync())
                {
                    Size += task.Current;

                    yield return task.Current;
                }
                else
                {
                    removed.Add(task);
                }

                token.ThrowIfCancellationRequested();
            }

            if (removed.IsEmpty())
            {
                continue;
            }

            tasks.Remove(removed);
            removed.Clear();

            if (tasks.IsEmpty())
            {
                break;
            }
        }
    }

    private void OrderDirectories()
    {
        if (Directories.Count == 0)
        {
            return;
        }

        var ordered = Directories.OrderByDescending(x => x.Size).ToArray();

        for (var index = 0; index < ordered.Length; index++)
        {
            if (Directories[index] == ordered[index])
            {
                continue;
            }

            Directories.Remove(Directories[index]);
            Directories.Insert(index, ordered[index]);
        }
    }

    private Task<DirectoryInfo[]> GetDirectoriesAsync(DirectoryInfo dir, CancellationToken token)
    {
        var task = Task.Run(
            () =>
            {
                try
                {
                    return dir.GetDirectories()
                        .Where(x => !x.Attributes.HasFlag(FileAttributes.ReparsePoint))
                        .OrderBy(x => x.Name)
                        .ToArray();
                }
                catch (UnauthorizedAccessException)
                {
                    return Array.Empty<DirectoryInfo>();
                }
            },
            token
        );

        return task;
    }

    private Task<IEnumerable<FileInfo>> GetFilesAsync(DirectoryInfo dir, CancellationToken token)
    {
        var task = Task.Run(
            () =>
            {
                try
                {
                    return dir.GetFiles()
                        .Where(x => !x.Attributes.HasFlag(FileAttributes.ReparsePoint));
                }
                catch (UnauthorizedAccessException)
                {
                    return Enumerable.Empty<FileInfo>();
                }
            },
            token
        );

        return task;
    }

    private Task<QuantitiesInformation> GetSizeAsync(FileInfo file, CancellationToken token)
    {
        var task = Task.Run(() => (QuantitiesInformation)file.Length, token);

        return task;
    }
}
