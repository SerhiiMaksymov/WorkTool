namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class AvaloniaMessageBoxView : IMessageBoxView
{
    public const     string    DialogControlName = "MainDialogControl";
    private readonly IResolver resolver;

    public AvaloniaMessageBoxView(IResolver resolver)
    {
        this.resolver = resolver.ThrowIfNull();
    }

    public Task ShowAsync(
    object                          title,
    object                          message,
    SystemColor                     background,
    IEnumerable<MessageBoxViewItem> messages)
    {
        switch (AvaloniaApplication.Current.ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
            {
                var window = desktop.Windows.FirstOrDefault(x => x.IsFocused)
                ?? desktop.MainWindow
                ?? desktop.Windows.First();

                var dialogControl = window.Content.As<DialogControl>();
                UpdateDialogControl(dialogControl, title, message, background, messages);

                return dialogControl.ShowAsync();
            }
            case ISingleViewApplicationLifetime single:
            {
                var dialogControl = single.MainView.As<ContentControl>().Content.As<DialogControl>();
                UpdateDialogControl(dialogControl, title, message, background, messages);

                return dialogControl.ShowAsync();
            }
            default:
            {
                throw new ArgumentException($"Unexpected {AvaloniaApplication.Current.ApplicationLifetime}.");
            }
        }
    }

    private void UpdateDialogControl(
    DialogControl                   dialogControl,
    object                          title,
    object                          message,
    SystemColor                     background,
    IEnumerable<MessageBoxViewItem> messages)
    {
        var messageView = resolver.Resolve<MessageView>()
            .AddArgumentValue(new ArgumentValue<IDialogView>(dialogControl));

        var buttons = messages.Select(
            x => new Button()
                .SetContent(x.Content)
                .SetCommand(messageView.CreateCommand(x.Action)));

        dialogControl.SetDialog(
                messageView.SetTitle(title)
                    .SetContent(message)
                    .AddItems(buttons))
            .SetDialogBackground(
                background.ToAvalonia()
                    .ToBrush());
    }
}