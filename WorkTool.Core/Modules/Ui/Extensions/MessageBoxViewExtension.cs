namespace WorkTool.Core.Modules.Ui.Extensions;

public static class MessageBoxViewExtension
{
    public static Task ShowErrorAsync<TMessageBoxView>(this TMessageBoxView messageBoxView, object message)
        where TMessageBoxView : IMessageBoxView
    {
        return messageBoxView.ShowAsync("Error", message, SystemColor.Red, MessageBoxViewItem.OkResults);
    }
}