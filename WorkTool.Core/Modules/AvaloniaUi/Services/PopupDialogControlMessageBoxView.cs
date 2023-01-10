namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class PopupDialogControlMessageBoxView : IMessageBoxView
{
    private readonly IResolver resolver;

    public PopupDialogControlMessageBoxView(IResolver resolver)
    {
        this.resolver = resolver;
    }

    public Task ShowAsync(
        object title,
        object message,
        SystemColor background,
        IEnumerable<MessageBoxViewItem> messages
    )
    {
        var avaloniaApplicationCurrent = AvaloniaApplication.Current.ThrowIfNull();
        var applicationLifetime = avaloniaApplicationCurrent.ApplicationLifetime.ThrowIfNull();

        switch (applicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
            {
                var window = GetDefaultWindow(desktop).ThrowIfIsNot<WindowPopup>();
                var popup  = window.Popup.ThrowIfNull();
                UpdatePopupControl(popup, title, message, background, messages);

                return popup.ShowAsync();
            }
            case ISingleViewApplicationLifetime single:
            {
                var popup = single.MainView.ThrowIfNull().FindControlThrowIfNotFound<PopupDialogControl>(WindowPopup.PartPopup);
                UpdatePopupControl(popup, title, message, background, messages);

                return popup.ShowAsync();
            }
            default:
            {
                throw new TypeInvalidCastException(
                    new[]
                    {
                        typeof(IClassicDesktopStyleApplicationLifetime),
                        typeof(ISingleViewApplicationLifetime)
                    },
                    applicationLifetime.GetType()
                );
            }
        }
    }

    private Window GetDefaultWindow(IClassicDesktopStyleApplicationLifetime desktop)
    {
        if (desktop.Windows.Count == 0)
        {
            return desktop.MainWindow.ThrowIfNull();
        }

        foreach (var window in desktop.Windows)
        {
            if (window.IsFocused)
            {
                return window;
            }
        }

        if (desktop.MainWindow is not null)
        {
            return desktop.MainWindow;
        }
        
        return desktop.Windows[0];
    }

    private void UpdatePopupControl(
        PopupDialogControl dialogControl,
        object title,
        object message,
        SystemColor background,
        IEnumerable<MessageBoxViewItem> messages
    )
    {
        var messageView = resolver.Resolve<MessageView>();
        messageView.SetParameter(typeof(IDialogView), dialogControl);

        var buttons = messages.Select(
            x => new Button().SetContent(x.Content).SetCommand(messageView.CreateCommand(x.Action))
        );

        dialogControl
            .SetContent(messageView.SetTitle(title).SetContent(message).AddItems(buttons))
            .SetBackground(background.ToAvalonia().ToBrush());
    }
}
