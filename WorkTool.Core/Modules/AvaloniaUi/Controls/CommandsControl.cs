namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class CommandsControl : ItemsControl
{
    public static readonly StyledProperty<object> ContentProperty =
        AvaloniaProperty.Register<CommandsControl, object>(nameof(Content));

    [Content]
    public object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
}