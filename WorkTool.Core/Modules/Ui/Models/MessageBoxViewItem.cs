namespace WorkTool.Core.Modules.Ui.Models;

public class MessageBoxViewItem
{
    public static readonly MessageBoxViewItem Ok = new (
        (IDialogView dialogView) => dialogView.CloseAsync(),
        "Ok");

    public static readonly IEnumerable<MessageBoxViewItem> OkResults = new[]
    {
        Ok
    };

    public Delegate Action  { get; }
    public object   Content { get; }

    public MessageBoxViewItem(Delegate action, object content)
    {
        Action  = action.ThrowIfNull();
        Content = content.ThrowIfNull();
    }
}