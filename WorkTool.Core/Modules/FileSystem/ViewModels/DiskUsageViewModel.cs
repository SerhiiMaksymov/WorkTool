namespace WorkTool.Core.Modules.FileSystem.ViewModels;

public class DiskUsageViewModel : ViewModelBase
{
    private QuantitiesInformation size;
    private string? directory;
    private DirectoryViewModel? selectedNode;
    private uint count;
    private ITaskCompletionSourceEnumerator taskCompletionSourceEnumerator;
    private CancellationTokenSource cancellationTokenSource;
    private IAsyncEnumerator<QuantitiesInformation>? task;
    private DirectoryViewModel? currentDirectory;
    private bool showEmptyFolgers;
    private IDirectoryService directoryService;

    public DiskUsageViewModel(
        IScheduler scheduler,
        IHumanizing<Exception, object> humanizing,
        IMessageBoxView messageBoxView,
        IInvoker invoker,
        IDirectoryService directoryService,
        ITaskCompletionSourceEnumerator taskCompletionSourceEnumerator
    ) : base(scheduler, humanizing, messageBoxView, invoker)
    {
        this.directoryService = directoryService;
        cancellationTokenSource = new();
        this.taskCompletionSourceEnumerator = taskCompletionSourceEnumerator;
        count = 500;
        Roots = new();
        directory = "/";
        StopCommand = CreateCommand(StopAsync);
        StartCommand = CreateCommand(StartAsync);
        ContinueCommand = CreateCommand(Continue);

        this.WhenAnyValue(x => x.SelectedNode)
            .Subscribe(x =>
            {
                if (x is null)
                {
                    return;
                }

                if (x.Directory is null)
                {
                    return;
                }

                Directory = x.Directory.ToPathString();
            });
    }

    public AvaloniaList<DirectoryViewModel> Roots { get; }
    public ICommand StartCommand { get; }
    public ICommand StopCommand { get; }
    public ICommand ContinueCommand { get; }

    public bool ShowEmptyFolgers
    {
        get => showEmptyFolgers;
        set => this.RaiseAndSetIfChanged(ref showEmptyFolgers, value);
    }

    public uint Count
    {
        get => count;
        set => this.RaiseAndSetIfChanged(ref count, value);
    }

    public DirectoryViewModel? SelectedNode
    {
        get => selectedNode;
        set => this.RaiseAndSetIfChanged(ref selectedNode, value);
    }

    public string? Directory
    {
        get => directory;
        set => this.RaiseAndSetIfChanged(ref directory, value);
    }

    public QuantitiesInformation Size
    {
        get => size;
        set => this.RaiseAndSetIfChanged(ref size, value);
    }

    private void Continue()
    {
        taskCompletionSourceEnumerator.MoveNext();
    }

    private async Task StartAsync()
    {
        var showEmptyFolgersValue = ShowEmptyFolgers;
        var maxCount = Count;
        uint currentCount = 0;
        Directory = Directory.ThrowIfNullOrWhiteSpace();

        var viewModel = currentDirectory ??= new DirectoryViewModel
        {
            Directory = new FileSystemDirectory(Directory, directoryService)
        };

        var currentTask = task ??= viewModel.RunAsync(
            showEmptyFolgersValue,
            cancellationTokenSource.Token
        );

        Roots.UpdateIfNeed(viewModel);

        while (await currentTask.MoveNextAsync())
        {
            Size += currentTask.Current;
            currentCount++;

            if (maxCount == currentCount)
            {
                CanExecute.OnNext(true);
                await taskCompletionSourceEnumerator.Current;
                CanExecute.OnNext(false);

                if (cancellationTokenSource.Token.IsCancellationRequested)
                {
                    return;
                }

                currentCount = 0;
            }
        }

        await MessageBoxView.ShowInfoAsync("End");
    }

    private async Task StopAsync()
    {
        cancellationTokenSource.Cancel();
        taskCompletionSourceEnumerator.MoveNext();
        cancellationTokenSource = new CancellationTokenSource();
        task = null;
        currentDirectory = null;
        await Dispatcher.UIThread.InvokeAsync(() => Roots.Clear());
    }
}
