namespace WorkTool.Core.XUnitTests.Fixtures;

public class AvaloniaFixture
{
    public static readonly IMutDependencyInjector Injector;
    public static readonly TestView View;
    protected readonly IScheduler Scheduler;

    static AvaloniaFixture()
    {
        var mutDependencyInjector = new MutDependencyInjector();
        mutDependencyInjector.RegisterConfigurationFromAssemblies();
        mutDependencyInjector.RegisterSingleton<Control, TestView>();
        Injector = mutDependencyInjector;
        View = (TestView)Injector.Resolve<Control>();
    }

    public AvaloniaFixture()
    {
        Scheduler = new TestScheduler();
    }

    public static Control? SetView(Type type)
    {
        var viewModel = View.DataContext.ThrowIfNull().ThrowIfIsNot<TestViewModel>();
        viewModel.Type = null;
        viewModel.Type = type;

        return viewModel.Content;
    }

    public static T? SetView<T>() where T : Control
    {
        return (T?)SetView(typeof(T));
    }

    public static void Stop()
    {
        var applicationLifetime = Application.Current.ThrowIfNull().ApplicationLifetime;

        if (applicationLifetime is IDisposable disposable)
        {
            Dispatcher.UIThread.Post(disposable.Dispose);
        }

        if (applicationLifetime is IControlledApplicationLifetime controlledApplicationLifetime)
        {
            Dispatcher.UIThread.Post(() => controlledApplicationLifetime.Shutdown());
        }
    }
}
