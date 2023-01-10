namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class MessageControl : ItemsControl
{
    public static readonly StyledProperty<object?> ContentProperty = AvaloniaProperty.Register<
        MessageControl,
        object?
    >(nameof(Content));

    public static readonly StyledProperty<object?> TitleProperty = AvaloniaProperty.Register<
        MessageControl,
        object?
    >(nameof(Title));

    public static readonly StyledProperty<IDataTemplate?> ContentTemplateProperty =
        AvaloniaProperty.Register<MessageControl, IDataTemplate?>(nameof(ContentTemplate));
    
    public static readonly StyledProperty<IBrush?> BackgroundTitleProperty =
        AvaloniaProperty.Register<MessageControl, IBrush?>(nameof(BackgroundTitle));

    public static readonly StyledProperty<IBrush?> BorderBrushTitleProperty =
        Border.BorderBrushProperty.AddOwner<MessageControl>();
    
    public static readonly StyledProperty<Thickness> BorderThicknessTitleProperty =
        Border.BorderThicknessProperty.AddOwner<MessageControl>();
    
    public static readonly StyledProperty<CornerRadius> CornerRadiusTitleProperty =
        Border.CornerRadiusProperty.AddOwner<MessageControl>();
    
    public static readonly StyledProperty<IDataTemplate?> ContentTemplateTitleProperty =
        AvaloniaProperty.Register<MessageControl, IDataTemplate?>(nameof(ContentTemplateTitle));

    public static readonly StyledProperty<Thickness> PaddingTitleProperty =
        Decorator.PaddingProperty.AddOwner<MessageControl>();
    
    public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentTitleProperty =
        AvaloniaProperty.Register<MessageControl, VerticalAlignment>(nameof(VerticalContentAlignmentTitle));
  
    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentTitleProperty =
        AvaloniaProperty.Register<MessageControl, HorizontalAlignment>(nameof(HorizontalContentAlignmentTitle));
    
    public static readonly StyledProperty<IBrush?> BackgroundContentProperty =
        AvaloniaProperty.Register<ContentControl, IBrush?>(nameof(BackgroundContent));
    
    public static readonly StyledProperty<IBrush?> BorderBrushContentProperty =
        Border.BorderBrushProperty.AddOwner<ContentControl>();
    
    public static readonly StyledProperty<Thickness> BorderThicknessContentProperty =
        Border.BorderThicknessProperty.AddOwner<ContentControl>();
    
    public static readonly StyledProperty<CornerRadius> CornerRadiusContentProperty =
        Border.CornerRadiusProperty.AddOwner<ContentControl>();
    
    public static readonly StyledProperty<IDataTemplate?> ContentTemplateContentProperty =
        AvaloniaProperty.Register<ContentControl, IDataTemplate?>(nameof(ContentTemplateContent));

    public static readonly StyledProperty<Thickness> PaddingContentProperty =
        Decorator.PaddingProperty.AddOwner<ContentControl>();
    
    public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentContentProperty =
        AvaloniaProperty.Register<ContentControl, VerticalAlignment>(nameof(VerticalContentAlignmentContent));
  
    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentContentProperty =
        AvaloniaProperty.Register<ContentControl, HorizontalAlignment>(nameof(HorizontalContentAlignmentContent));
    
    public HorizontalAlignment HorizontalContentAlignmentContent
    {
        get => GetValue(HorizontalContentAlignmentContentProperty);
        set => SetValue(HorizontalContentAlignmentContentProperty, value);
    }
    
    public VerticalAlignment VerticalContentAlignmentContent
    {
        get => GetValue(VerticalContentAlignmentContentProperty);
        set => SetValue(VerticalContentAlignmentContentProperty, value);
    }
    
    public Thickness PaddingContent
    {
        get => GetValue(PaddingContentProperty);
        set => SetValue(PaddingContentProperty, value);
    }
    
    public IDataTemplate? ContentTemplateContent
    {
        get => GetValue(ContentTemplateContentProperty);
        set => SetValue(ContentTemplateContentProperty, value);
    }
    
    public CornerRadius CornerRadiusContent
    {
        get => GetValue(CornerRadiusContentProperty);
        set => SetValue(CornerRadiusContentProperty, value);
    }
    
    public Thickness BorderThicknessContent
    {
        get => GetValue(BorderThicknessContentProperty);
        set => SetValue(BorderThicknessContentProperty, value);
    }
    
    public IBrush? BorderBrushContent
    {
        get => GetValue(BorderBrushContentProperty);
        set => SetValue(BorderBrushContentProperty, value);
    }

    public IBrush? BackgroundContent
    {
        get => GetValue(BackgroundContentProperty);
        set => SetValue(BackgroundContentProperty, value);
    }
    
    public HorizontalAlignment HorizontalContentAlignmentTitle
    {
        get => GetValue(HorizontalContentAlignmentTitleProperty);
        set => SetValue(HorizontalContentAlignmentTitleProperty, value);
    }
    
    public VerticalAlignment VerticalContentAlignmentTitle
    {
        get => GetValue(VerticalContentAlignmentTitleProperty);
        set => SetValue(VerticalContentAlignmentTitleProperty, value);
    }
    
    public Thickness PaddingTitle
    {
        get => GetValue(PaddingTitleProperty);
        set => SetValue(PaddingTitleProperty, value);
    }
    
    public IDataTemplate? ContentTemplateTitle
    {
        get => GetValue(ContentTemplateTitleProperty);
        set => SetValue(ContentTemplateTitleProperty, value);
    }
    
    public CornerRadius CornerRadiusTitle
    {
        get => GetValue(CornerRadiusTitleProperty);
        set => SetValue(CornerRadiusTitleProperty, value);
    }
    
    public Thickness BorderThicknessTitle
    {
        get => GetValue(BorderThicknessTitleProperty);
        set => SetValue(BorderThicknessTitleProperty, value);
    }
    
    public IBrush? BorderBrushTitle
    {
        get => GetValue(BorderBrushTitleProperty);
        set => SetValue(BorderBrushTitleProperty, value);
    }

    public IBrush? BackgroundTitle
    {
        get => GetValue(BackgroundTitleProperty);
        set => SetValue(BackgroundTitleProperty, value);
    }

    [Content]
    [DependsOn(nameof(ContentTemplate))]
    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public IDataTemplate? ContentTemplate
    {
        get => GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }

    public object? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
}
