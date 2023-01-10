namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class NotificationMessageBoxView : IMessageBoxView
{
    private readonly IManagedNotificationManager managedNotificationManager;

    public NotificationMessageBoxView(
        IManagedNotificationManager managedNotificationManager,
        IResolver resolver
    )
    {
        resolver.ThrowIfNull();
        this.managedNotificationManager = managedNotificationManager.ThrowIfNull();
    }

    public Task ShowAsync(
        object title,
        object message,
        SystemColor background,
        IEnumerable<MessageBoxViewItem> messages
    )
    {
        managedNotificationManager.Show(
            new AvaloniaNotification("Test", message.ToString(), NotificationType.Warning)
        );

        return Task.CompletedTask;
    }
}
