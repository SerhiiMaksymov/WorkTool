namespace WorkTool.Core.Modules.FileSystem.ViewModel;

public class DiskUsageViewModel : ViewModelBase
{
    private QuantitiesInformation size;
    private string? directory;
    private CancellationTokenSource cancellationTokenSource;
    private DirectoryViewModel? selectedNode;

    public DiskUsageViewModel(
        IHumanizing<Exception, object> humanizing,
        IMessageBoxView messageBoxView
    ) : base(humanizing, messageBoxView)
    {
        cancellationTokenSource = new();
        Roots = new();
        directory = "/";
        StopCommand = CreateCommand(RefreshCancellationToken);

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

                Directory = x.Directory.FullName;
            });

        StartCommand = ReactiveCommand.Create(() =>
        {
            RefreshCancellationToken();

            return RefreshAsync(cancellationTokenSource.Token);
        });
    }

    public AvaloniaList<DirectoryViewModel> Roots { get; }
    public ICommand StartCommand { get; }
    public ICommand StopCommand { get; }

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

    private async Task RefreshAsync(CancellationToken token)
    {
        Directory = Directory.ThrowIfNullOrWhiteSpace();

        var viewModel = new DirectoryViewModel { Directory = new DirectoryInfo(Directory) };

        var task = viewModel.RunAsync(token);
        Roots.Update(viewModel);

        while (await task.MoveNextAsync())
        {
            Size += task.Current;
        }
    }

    private void RefreshCancellationToken()
    {
        cancellationTokenSource.Cancel();
        cancellationTokenSource = new CancellationTokenSource();
    }
}
