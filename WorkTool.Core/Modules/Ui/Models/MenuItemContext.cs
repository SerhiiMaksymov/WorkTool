namespace WorkTool.Core.Modules.Ui.Models;

public class MenuItemContext
{
    public Delegate Task { get; }
    public Func<object> Header { get; }

    public MenuItemContext(Delegate task, Func<object> header)
    {
        Task = task;
        Header = header.ThrowIfNull();
    }
}
