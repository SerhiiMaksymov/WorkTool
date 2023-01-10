namespace WorkTool.Core.Modules.Ui.Extensions;

public static class MessageBoxViewExtension
{
    public static Task ShowErrorAsync<TMessageBoxView>(
        this TMessageBoxView messageBoxView,
        object message
    ) where TMessageBoxView : IMessageBoxView
    {
        return messageBoxView.ShowAsync(
            "Error",
            message,
            SystemColor.Red,
            MessageBoxViewItem.OkResults
        );
    }

    public static Task ShowInfoAsync<TMessageBoxView>(
        this TMessageBoxView messageBoxView,
        object message
    ) where TMessageBoxView : IMessageBoxView
    {
        return messageBoxView.ShowAsync(
            "Info",
            message,
            SystemColor.Aquamarine,
            MessageBoxViewItem.OkResults
        );
    }
}
