namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class PropertyInfoTemplatedControl<TValue, TControl>
    : TemplatedControl,
        IDisposable where TControl : AvaloniaObject, new()
{
    public const string ElementControlName = "PART_Control";
    public const string ElementTextBlockName = "PART_TextBlock";
    
    public static readonly DirectProperty< PropertyInfoTemplatedControl<TValue, TControl>, object?> ObjectProperty =
        AvaloniaProperty.RegisterDirect< PropertyInfoTemplatedControl<TValue, TControl>, object?>(
            nameof(Object),
            o => o.Object,
            (o, v) => o.Object = v
        );


    public static readonly DirectProperty<
        PropertyInfoTemplatedControl<TValue, TControl>,
        string?
    > TitleProperty = AvaloniaProperty.RegisterDirect<
        PropertyInfoTemplatedControl<TValue, TControl>,
        string?
    >(nameof(Title), o => o.Title, (o, v) => o.Title = v);

    public static readonly DirectProperty<
        PropertyInfoTemplatedControl<TValue, TControl>,
        PropertyInfo?
    > PropertyInfoProperty = AvaloniaProperty.RegisterDirect<
        PropertyInfoTemplatedControl<TValue, TControl>,
        PropertyInfo?
    >(nameof(PropertyInfo), o => o.PropertyInfo, (o, v) => o.PropertyInfo = v);

    public static readonly DirectProperty<
        PropertyInfoTemplatedControl<TValue?, TControl>,
        TValue?
    > ValueProperty = AvaloniaProperty.RegisterDirect<
        PropertyInfoTemplatedControl<TValue?, TControl>,
        TValue?
    >(nameof(Value), o => o.Value, (o, v) => o.Value = v);
    private readonly Action<
        PropertyInfoTemplatedControl<TValue, TControl>,
        TemplateAppliedEventArgs,
        TControl,
        TextBlock
    > onApplyTemplate;
    private readonly CompositeDisposable compositeDisposable;
    private TControl? control;
    private object? @object;
    private PropertyInfo? propertyInfo;
    private TextBlock? textBlock;
    private string? title;
    private TValue? value;

    public string? Title
    {
        get => title;
        set => SetAndRaise(TitleProperty, ref title, value);
    }

    public PropertyInfo? PropertyInfo
    {
        get => propertyInfo;
        set => SetAndRaise(PropertyInfoProperty, ref propertyInfo, value);
    }

    public TValue? Value
    {
        get => value;
#pragma warning disable CS8601
        set => SetAndRaise(ValueProperty, ref this.value, value);
#pragma warning restore CS8601
    }

    static PropertyInfoTemplatedControl()
    {
        PropertyInfoProperty.Changed.AddClassHandler<
            PropertyInfoTemplatedControl<TValue, TControl>
        >((_, e) => PropertyInfoChanged(e));

        ObjectProperty.Changed.AddClassHandler<
            PropertyInfoTemplatedControl<TValue, TControl>
        >((_, e) => ObjectChanged(e));

        ValueProperty.Changed.AddClassHandler<PropertyInfoTemplatedControl<TValue, TControl>>(
            (_, e) => ValueChanged(e)
        );
    }

    public PropertyInfoTemplatedControl(
        Action<
            PropertyInfoTemplatedControl<TValue, TControl>,
            TemplateAppliedEventArgs,
            TControl,
            TextBlock
        > onApplyTemplate
    )
    {
        compositeDisposable = new CompositeDisposable();
        this.onApplyTemplate = onApplyTemplate.ThrowIfNull();
    }

    public void Dispose()
    {
        compositeDisposable.Dispose();
    }

    public object? Object
    {
        get => @object;
        set => SetAndRaise(ObjectProperty, ref @object, value);
    }

    private static void ValueChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var sender = (PropertyInfoTemplatedControl<TValue, TControl>)e.Sender;

        if (sender.PropertyInfo is null)
        {
            return;
        }

        if (sender.Object is null)
        {
            return;
        }

        if (sender.PropertyInfo.GetAccessors().Any(x => x.IsStatic))
        {
            return;
        }

        sender.PropertyInfo.SetValue(sender.Object, e.NewValue);
    }

    private static void PropertyInfoChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is null)
        {
            return;
        }

        var sender = (PropertyInfoTemplatedControl<TValue, TControl>)e.Sender;
        var propertyInfo = (PropertyInfo)e.NewValue;

        if (sender.Object is null)
        {
            return;
        }

        if (propertyInfo.GetAccessors().Any(x => x.IsStatic))
        {
            return;
        }

        propertyInfo.SetValue(sender.Object, sender.Value);
    }

    private static void ObjectChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var sender = (PropertyInfoTemplatedControl<TValue, TControl>)e.Sender;

        if (sender.PropertyInfo is null)
        {
            return;
        }

        if (e.NewValue is null)
        {
            return;
        }

        if (sender.PropertyInfo.GetAccessors().Any(x => x.IsStatic))
        {
            return;
        }

        sender.PropertyInfo.SetValue(e.NewValue, sender.Value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs templateAppliedEventArgs)
    {
        base.OnApplyTemplate(templateAppliedEventArgs);
        control = templateAppliedEventArgs.NameScope.Get<TControl>(ElementControlName);
        textBlock = templateAppliedEventArgs.NameScope.Get<TextBlock>(ElementTextBlockName);
        onApplyTemplate.Invoke(this, templateAppliedEventArgs, control, textBlock);
    }

    public void AddDisposable<TDisposable>(TDisposable disposable) where TDisposable : IDisposable
    {
        compositeDisposable.Add(disposable);
    }
}
