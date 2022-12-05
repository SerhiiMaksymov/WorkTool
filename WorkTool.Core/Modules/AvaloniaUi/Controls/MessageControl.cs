namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class MessageControl : ItemsControl
{
    public static readonly StyledProperty<object> ContentProperty = AvaloniaProperty.Register<
        MessageControl,
        object
    >(nameof(Content));

    public static readonly StyledProperty<object> TitleProperty = AvaloniaProperty.Register<
        MessageControl,
        object
    >(nameof(Title));

    public static readonly StyledProperty<IDataTemplate> ContentTemplateProperty =
        AvaloniaProperty.Register<MessageControl, IDataTemplate>(nameof(ContentTemplate));

    [Content]
    [DependsOn(nameof(ContentTemplate))]
    public object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public IDataTemplate ContentTemplate
    {
        get => GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }

    public object Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
}
