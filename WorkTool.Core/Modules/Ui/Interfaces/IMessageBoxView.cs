namespace WorkTool.Core.Modules.Ui.Interfaces;

public interface IMessageBoxView
{
    Task ShowAsync(
        object title,
        object message,
        SystemColor background,
        IEnumerable<MessageBoxViewItem> messages
    );
}
