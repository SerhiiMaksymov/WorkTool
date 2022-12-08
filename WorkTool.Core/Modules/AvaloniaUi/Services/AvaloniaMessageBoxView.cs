namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class AvaloniaMessageBoxView : IMessageBoxView
{
    public const string DialogControlName = "MainDialogControl";
    private readonly IResolver resolver;

    public AvaloniaMessageBoxView(IResolver resolver)
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
                var window =
                    desktop.Windows.FirstOrDefault(x => x.IsFocused)
                    ?? desktop.MainWindow
                    ?? desktop.Windows.First();

                var content = window.Content.ThrowIfNull();

                if (content is not DialogControl dialogControl)
                {
                    throw new TypeInvalidCastException(typeof(DialogControl), content.GetType());
                }

                UpdateDialogControl(dialogControl, title, message, background, messages);

                return dialogControl.ShowAsync();
            }
            case ISingleViewApplicationLifetime single:
            {
                var mainView = single.MainView.ThrowIfNull();

                if (mainView is not ContentControl contentControl)
                {
                    throw new TypeInvalidCastException(typeof(ContentControl), mainView.GetType());
                }

                var content = contentControl.Content.ThrowIfNull();

                if (content is not DialogControl dialogControl)
                {
                    throw new TypeInvalidCastException(typeof(DialogControl), content.GetType());
                }

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

    private void UpdateDialogControl(
        DialogControl dialogControl,
        object title,
        object message,
        SystemColor background,
        IEnumerable<MessageBoxViewItem> messages
    )
    {
        var messageView = resolver
            .Resolve<MessageView>()
            .AddArgumentValue(new ArgumentValue<IDialogView>(dialogControl));

        var buttons = messages.Select(
            x => new Button().SetContent(x.Content).SetCommand(messageView.CreateCommand(x.Action))
        );

        dialogControl
            .SetDialog(messageView.SetTitle(title).SetContent(message).AddItems(buttons))
            .SetDialogBackground(background.ToAvalonia().ToBrush());
    }
}
