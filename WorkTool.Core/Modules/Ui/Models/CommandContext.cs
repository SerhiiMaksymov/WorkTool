namespace WorkTool.Core.Modules.Ui.Models;

public class CommandContext
{
    public Delegate Task { get; }
    public Func<object> Content { get; }

    public CommandContext(Delegate task, Func<object> content)
    {
        Task = task.ThrowIfNull();
        Content = content.ThrowIfNull();
    }
}
