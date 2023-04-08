namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

[TemplatePart(ElementItemsPresenter, typeof(ItemsPresenter))]
public class PropertyInfoReactiveItemsControl : ItemsControl, IActivatableView
{
    public const string ElementItemsPresenter = "PART_ItemsPresenter";
    public const string ElementTextBlock = "PART_TextBlock";
    
    public static readonly DirectProperty<PropertyInfoReactiveItemsControl, object?> ObjectProperty =
        AvaloniaProperty.RegisterDirect< PropertyInfoReactiveItemsControl, object?>(
            nameof(Object),
            o => o.Object,
            (o, v) => o.Object = v
        );

    public static readonly DirectProperty<PropertyInfoReactiveItemsControl, string?> TitleProperty =
        AvaloniaProperty.RegisterDirect<PropertyInfoReactiveItemsControl, string?>(
            nameof(Title),
            o => o.Title,
            (o, v) => o.Title = v
        );

    public static readonly DirectProperty<
        PropertyInfoReactiveItemsControl,
        PropertyInfo?
    > PropertyInfoProperty = AvaloniaProperty.RegisterDirect<
        PropertyInfoReactiveItemsControl,
        PropertyInfo?
    >(nameof(PropertyInfo), o => o.PropertyInfo, (o, v) => o.PropertyInfo = v);

    public static readonly DirectProperty<PropertyInfoReactiveItemsControl, object?> ValueProperty =
        AvaloniaProperty.RegisterDirect<PropertyInfoReactiveItemsControl, object?>(
            nameof(Value),
            o => o.Value,
            (o, v) => o.Value = v
        );
    protected readonly AvaloniaList<object> ItemProperties;
    protected CompositeDisposable CompositeDisposable;
    private object? @object;
    private PropertyInfo? propertyInfo;
    private string? title;
    private object? value;

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

    public object? Value
    {
        get => value;
        set => SetAndRaise(ValueProperty, ref this.value, value);
    }

    static PropertyInfoReactiveItemsControl()
    {
        ItemTemplateProperty.AddOwner<PropertyInfoReactiveItemsControl>();
        ItemsProperty.AddOwner<PropertyInfoReactiveItemsControl>(x => x.Items);
        ItemsPanelProperty.AddOwner<PropertyInfoReactiveItemsControl>();
        ObjectProperty.AddOwner<PropertyInfoReactiveItemsControl>(x => x.Object);

        PropertyInfoProperty.Changed.AddClassHandler<PropertyInfoReactiveItemsControl>(
            (_, e) => PropertyInfoChanged(e)
        );

        ObjectProperty.Changed.AddClassHandler<PropertyInfoReactiveItemsControl>(
            (_, e) => ObjectChanged(e)
        );

        ValueProperty.Changed.AddClassHandler<PropertyInfoReactiveItemsControl>(
            (_, e) => ValueChanged(e)
        );
    }

    public PropertyInfoReactiveItemsControl()
    {
        CompositeDisposable = new CompositeDisposable();
        this.WhenActivated(disposables => CompositeDisposable.DisposeWith(disposables));
        var items = Items.ThrowIfNull();

        if (items is AvaloniaList<object> avaloniaList)
        {
            ItemProperties = avaloniaList;
        }
        else
        {
            throw new TypeInvalidCastException(typeof(AvaloniaList<object>), items.GetType());
        }
    }

    public object? Object
    {
        get => @object;
        set => SetAndRaise(ObjectProperty, ref @object, value);
    }

    protected virtual void UpdateItems() { }

    private static void ValueChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var sender = (PropertyInfoReactiveItemsControl)e.Sender;
        sender.UpdateItems();

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

        var sender = (PropertyInfoReactiveItemsControl)e.Sender;
        var propertyInfo = (PropertyInfo)e.NewValue;
        sender.UpdateItems();

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
        var sender = (PropertyInfoReactiveItemsControl)e.Sender;
        sender.UpdateItems();

        if (e.NewValue is null)
        {
            return;
        }

        if (sender.PropertyInfo is null)
        {
            return;
        }

        if (sender.PropertyInfo.GetAccessors().Any(x => x.IsStatic))
        {
            return;
        }

        sender.PropertyInfo.SetValue(e.NewValue, sender.Value);
    }
}
