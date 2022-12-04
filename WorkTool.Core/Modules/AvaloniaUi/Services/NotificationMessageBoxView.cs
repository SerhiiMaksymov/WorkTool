namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class NotificationMessageBoxView : IMessageBoxView
{
    private readonly IManagedNotificationManager _managedNotificationManager;
    private readonly IResolver                   _resolver;

    public NotificationMessageBoxView(IManagedNotificationManager managedNotificationManager, IResolver resolver)
    {
        _resolver                   = resolver.ThrowIfNull();
        _managedNotificationManager = managedNotificationManager.ThrowIfNull();
    }

    public Task ShowAsync(object                          title, object message, SystemColor background,
                          IEnumerable<MessageBoxViewItem> messages)
    {
        _managedNotificationManager.Show(new Notification("Test", message.ToString(), NotificationType.Warning));

        return Task.CompletedTask;
    }
}