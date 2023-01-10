namespace WorkTool.Core.Modules.UnitTest.Services;

public class AvaloniaTest : IDisposable
{
    private readonly AvaloniaUiApplication application;

    public AvaloniaTest(AvaloniaUiApplication application)
    {
        this.application = application;
    }

    public void Dispose()
    {
        var app =
            AvaloniaApplication.Current.ApplicationLifetime.ThrowIfIsNot<IControlledApplicationLifetime>();

        if (app is IDisposable disposable)
        {
            Dispatcher.UIThread.Post(disposable.Dispose);
        }

        Dispatcher.UIThread.Post(() => app.Shutdown());
    }

    public void SyncContext()
    {
        var tcs = new TaskCompletionSource<SynchronizationContext>();
        var thread = new Thread(() =>
        {
            try
            {
                application.Run(Array.Empty<string>());
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        })
        {
            IsBackground = true
        };

        thread.Start();
        SynchronizationContext.SetSynchronizationContext(tcs.Task.Result);
    }
}
