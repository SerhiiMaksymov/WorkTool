namespace WorkTool.Core.Modules.Ui.Extensions;

public static class MessageBoxViewExtension
{
    public static Task ShowAsync<TMessageBoxView>(
        this TMessageBoxView messageBoxView,
        object title,
        object message,
        IEnumerable<MessageBoxViewItem> messages
    ) where TMessageBoxView : IMessageBoxView
    {
        return messageBoxView.ShowAsync(title, message, ColorHelper.Background, messages);
    }

    public static async Task<bool> ShowBooleanAsync<TMessageBoxView>(
        this TMessageBoxView messageBoxView,
        object title,
        object message
    ) where TMessageBoxView : IMessageBoxView
    {
        var result = false;

        await messageBoxView.ShowAsync(
            title,
            message,
            new[]
            {
                new MessageBoxViewItem(
                    (IDialogView dialogView) =>
                    {
                        result = true;

                        return dialogView.CloseAsync();
                    },
                    "Ok"
                ),
                new MessageBoxViewItem(
                    (IDialogView dialogView) =>
                    {
                        result = false;

                        return dialogView.CloseAsync();
                    },
                    "Cancel"
                ),
            }
        );

        return result;
    }

    public static Task ShowErrorAsync<TMessageBoxView>(
        this TMessageBoxView messageBoxView,
        object message
    ) where TMessageBoxView : IMessageBoxView
    {
        return messageBoxView.ShowAsync(
            "Error",
            message,
            ColorHelper.Error,
            MessageBoxViewItem.OkResults
        );
    }

    public static Task ShowInfoAsync<TMessageBoxView>(
        this TMessageBoxView messageBoxView,
        object message
    ) where TMessageBoxView : IMessageBoxView
    {
        return messageBoxView.ShowInfoAsync("Info", message);
    }

    public static Task ShowInfoAsync<TMessageBoxView>(
        this TMessageBoxView messageBoxView,
        object title,
        object message
    ) where TMessageBoxView : IMessageBoxView
    {
        return messageBoxView.ShowAsync(
            title,
            message,
            ColorHelper.Info,
            MessageBoxViewItem.OkResults
        );
    }
}
