namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class ExceptionHumanizing : IHumanizing<Exception, object>
{
    private readonly IHumanizing<Exception, string> humanizing;

    public ExceptionHumanizing(IHumanizing<Exception, string> humanizing)
    {
        this.humanizing = humanizing.ThrowIfNull();
    }

    public object Humanize(Exception input)
    {
        var text = humanizing.Humanize(input);

        return new TextBox()
            .SetAcceptsReturn(true)
            .SetTextWrapping(TextWrapping.Wrap)
            .SetText(text);
    }
}