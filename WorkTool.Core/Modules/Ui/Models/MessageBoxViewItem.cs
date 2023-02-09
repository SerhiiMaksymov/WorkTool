namespace WorkTool.Core.Modules.Ui.Models;

public class MessageBoxViewItem
{
    public static readonly MessageBoxViewItem Ok =
        new((IDialogView dialogView) => dialogView.CloseAsync(), "Ok");

    public static readonly MessageBoxViewItem Cancel =
        new((IDialogView dialogView) => dialogView.CloseAsync(), "Cancel");

    public static readonly IEnumerable<MessageBoxViewItem> OkResults = new[] { Ok };
    public static readonly IEnumerable<MessageBoxViewItem> OkCancelResults = new[] { Ok, Cancel };

    public Delegate Action { get; }
    public object Content { get; }

    public MessageBoxViewItem(Delegate action, object content)
    {
        Action = action.ThrowIfNull();
        Content = content.ThrowIfNull();
    }
}
