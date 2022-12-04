namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

[TemplatePart(ElementItemsPresenter, typeof(ItemsPresenter))]
public class PropertyInfoReactiveItemsControl : ItemsControl, IObjectValue, IActivatableView
{
    public const string ElementItemsPresenter = "PART_ItemsPresenter";
    public const string ElementTextBlock      = "PART_TextBlock";

    public static readonly DirectProperty<PropertyInfoReactiveItemsControl, string> TitleProperty =
        AvaloniaProperty.RegisterDirect<PropertyInfoReactiveItemsControl, string>(
            nameof(Title),
            o => o.Title,
            (o, v) => o.Title = v);

    public static readonly DirectProperty<PropertyInfoReactiveItemsControl, PropertyInfo> PropertyInfoProperty =
        AvaloniaProperty.RegisterDirect<PropertyInfoReactiveItemsControl, PropertyInfo>(
            nameof(PropertyInfo),
            o => o.PropertyInfo,
            (o, v) => o.PropertyInfo = v);

    public static readonly DirectProperty<PropertyInfoReactiveItemsControl, object> ValueProperty =
        AvaloniaProperty.RegisterDirect<PropertyInfoReactiveItemsControl, object>(
            nameof(Value),
            o => o.Value,
            (o, v) => o.Value = v);
    protected readonly AvaloniaList<object> ItemProperties;
    protected          CompositeDisposable  CompositeDisposable;
    private            object               @object;
    private            PropertyInfo         propertyInfo;
    private            string               title;
    private            object               value;

    public string Title
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
        IObjectValue.ObjectProperty.AddOwner<PropertyInfoReactiveItemsControl>(x => x.Object);

        PropertyInfoProperty.Changed.AddClassHandler<PropertyInfoReactiveItemsControl>(
            (_, e) => PropertyInfoChanged(e));

        IObjectValue.ObjectProperty.Changed.AddClassHandler<PropertyInfoReactiveItemsControl>(
            (_, e) => ObjectChanged(e));

        ValueProperty.Changed.AddClassHandler<PropertyInfoReactiveItemsControl>((_, e) => ValueChanged(e));
    }

    public PropertyInfoReactiveItemsControl()
    {
        CompositeDisposable = new CompositeDisposable();
        this.WhenActivated(disposables => CompositeDisposable.DisposeWith(disposables));
        ItemProperties = (AvaloniaList<object>)Items;
    }

    public object? Object
    {
        get => @object;
        set => SetAndRaise(IObjectValue.ObjectProperty, ref @object, value);
    }

    protected virtual void UpdateItems()
    {
    }

    private static void ValueChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var sender = (PropertyInfoReactiveItemsControl)e.Sender;

        if (sender is null)
        {
            return;
        }

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

        var sender       = (PropertyInfoReactiveItemsControl)e.Sender;
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