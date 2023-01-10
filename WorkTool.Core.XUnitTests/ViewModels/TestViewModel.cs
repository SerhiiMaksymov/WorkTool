namespace WorkTool.Core.XUnitTests.ViewModels;

public class TestViewModel : ViewModelBase
{
    private readonly IResolver resolver;
    private Control? content;
    private Type? type;

    public TestViewModel(
        IScheduler scheduler,
        IHumanizing<Exception, object> humanizing,
        IMessageBoxView messageBoxView,
        IResolver resolver
    ) : base(scheduler, humanizing, messageBoxView)
    {
        this.resolver = resolver;

        this.WhenAnyValue(x => x.Type)
            .Subscribe(x =>
            {
                if (x is null)
                {
                    Content = null;

                    return;
                }

                Content = (Control)this.resolver.Resolve(x);
            });
    }

    public Type? Type
    {
        get => type;
        set => this.RaiseAndSetIfChanged(ref type, value);
    }

    public Control? Content
    {
        get => content;
        set => this.RaiseAndSetIfChanged(ref content, value);
    }
}
