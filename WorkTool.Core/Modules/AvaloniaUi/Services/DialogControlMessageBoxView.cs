namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class DialogControlMessageBoxView : IMessageBoxView
{
    public const string DialogControlName = "MainDialogControl";
    
    private readonly IResolver resolver;

    public DialogControlMessageBoxView(IResolver resolver)
    {
        this.resolver = resolver.ThrowIfNull();
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
                var window  = GetDefaultWindow(desktop);

                if (!DialogControl.Windows.TryGetValue(window, out var dialogControl))
                {
                    throw new NotFoundException();
                }

                UpdateDialogControl(dialogControl, title, message, background, messages);

                return dialogControl.ShowAsync();
            }
            case ISingleViewApplicationLifetime:
            {
                var dialogControl = DialogControl.LastDialogControl.ThrowIfNull();
                UpdateDialogControl(dialogControl, title, message, background, messages);

                return dialogControl.ShowAsync();
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

    private void UpdateDialogControl(
        DialogControl dialogControl,
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
            .SetDialog(messageView.SetTitle(title).SetContent(message).AddItems(buttons))
            .SetBackgroundDialog(background.ToAvalonia().ToBrush());
    }
}
