namespace WorkTool.Core.Modules.Ui.Models;

public class TabItemContext
{
    public Func<object> Header  { get; }
    public Func<object> Content { get; }

    public TabItemContext(Func<object> header, Func<object> content)
    {
        Header  = header.ThrowIfNull();
        Content = content.ThrowIfNull();
    }
}