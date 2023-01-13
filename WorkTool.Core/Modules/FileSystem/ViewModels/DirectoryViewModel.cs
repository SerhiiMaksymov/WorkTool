namespace WorkTool.Core.Modules.FileSystem.ViewModels;

public class DirectoryViewModel : NotifyBase
{
    private QuantitiesInformation size;
    private IDirectory? directory;

    public DirectoryViewModel()
    {
        Directories = new();
    }

    public string? Name => directory?.Name;
    public AvaloniaList<DirectoryViewModel> Directories { get; }

    public IDirectory? Directory
    {
        get => directory;
        set => this.RaiseAndSetIfChanged(ref directory, value);
    }

    public QuantitiesInformation Size
    {
        get => size;
        set => this.RaiseAndSetIfChanged(ref size, value);
    }

    public async IAsyncEnumerator<QuantitiesInformation> RunAsync(
        bool showEmptyFolgers,
        CancellationToken token = default
    )
    {
        await Task.Yield();
        Directory = Directory.ThrowIfNull();
        var files = Directory.GetFiles();

        if (token.IsCancellationRequested)
        {
            yield break;
        }

        foreach (var file in files)
        {
            $"File:{file.ToPathString()}".ToConsoleLine();

            if (token.IsCancellationRequested)
            {
                yield break;
            }

            Size += file.Size;

            yield return file.Size;
        }

        var directories = Directory.GetDirectories().ToArray();

        if (token.IsCancellationRequested)
        {
            yield break;
        }

        var tasks =
            new List<(
                DirectoryViewModel Directory,
                IAsyncEnumerator<QuantitiesInformation> Enumerator
            )>();

        foreach (var dir in directories)
        {
            $"Dir:{dir.ToPathString()}".ToConsoleLine();
            var viewModel = new DirectoryViewModel { Directory = dir };

            if (showEmptyFolgers)
            {
                Directories.Add(viewModel);
            }

            tasks.Add((viewModel, viewModel.RunAsync(showEmptyFolgers, token)));
        }

        if (directories.IsEmpty())
        {
            yield break;
        }

        var removed =
            new List<(
                DirectoryViewModel Directory,
                IAsyncEnumerator<QuantitiesInformation> Enumerator
            )>();

        while (true)
        {
            foreach (var task in tasks)
            {
                if (await task.Enumerator.MoveNextAsync())
                {
                    var newSize = task.Enumerator.Current;

                    if (newSize == 0ul)
                    {
                        continue;
                    }

                    Size += newSize;

                    if (!showEmptyFolgers)
                    {
                        Directories.AddIfNotContains(task.Directory);
                    }

                    yield return newSize;

                    OrderDirectories();
                }
                else
                {
                    removed.Add(task);
                }

                if (token.IsCancellationRequested)
                {
                    yield break;
                }
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

    private DirectoryInfo[] GetDirectories(DirectoryInfo dir)
    {
        try
        {
            var directories = dir.GetDirectories()
                .Where(x => !x.Attributes.HasFlag(FileAttributes.ReparsePoint))
                .OrderBy(x => x.Name)
                .ToArray();

            return directories;
        }
        catch (UnauthorizedAccessException)
        {
            return Array.Empty<DirectoryInfo>();
        }
        catch (DirectoryNotFoundException)
        {
            return Array.Empty<DirectoryInfo>();
        }
    }

    private FileInfo[] GetFiles(DirectoryInfo dir)
    {
        try
        {
            var files = dir.GetFiles()
                .Where(x => !x.Attributes.HasFlag(FileAttributes.ReparsePoint))
                .ToArray();

            return files;
        }
        catch (UnauthorizedAccessException)
        {
            return Array.Empty<FileInfo>();
        }
        catch (DirectoryNotFoundException)
        {
            return Array.Empty<FileInfo>();
        }
    }

    private QuantitiesInformation GetSize(FileInfo file)
    {
        var fileSize = (QuantitiesInformation)file.Length;

        return fileSize;
    }
}
