namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class AvaloniaObjectExtension
{
    public static DisposableItem<TAvaloniaObject> BindDynamicResourceValue<TAvaloniaObject>(this TAvaloniaObject target,
        AvaloniaProperty targetProperty,
        object resourceKey)
        where TAvaloniaObject : IAvaloniaObject
    {
        var dynamicResourceBinding = new DynamicResourceBinding(resourceKey);

        return target.BindValue(targetProperty, dynamicResourceBinding);
    }

    public static DisposableItem<TAvaloniaObject> BindBindingValue<TAvaloniaObject>(
    this TAvaloniaObject target,
    AvaloniaProperty     targetProperty,
    Expression           expression)
        where TAvaloniaObject : IAvaloniaObject
    {
        var path    = expression.GetBindingPath();
        var binding = new Binding(path);

        return target.BindValue(targetProperty, binding);
    }

    public static DisposableItem<TAvaloniaObject> BindBindingValue<TAvaloniaObject>(
    this TAvaloniaObject target,
    AvaloniaProperty     targetProperty,
    Expression           expression,
    string               stringFormat)
        where TAvaloniaObject : IAvaloniaObject
    {
        var path = expression.GetBindingPath();

        var binding = new Binding(path)
            .SetStringFormat(stringFormat);

        return target.BindValue(targetProperty, binding, null);
    }

    public static DisposableItem<TAvaloniaObject> BindBindingValue<TAvaloniaObject>(
    this TAvaloniaObject target,
    AvaloniaProperty     targetProperty,
    Expression           expression,
    IValueConverter      converter)
        where TAvaloniaObject : IAvaloniaObject
    {
        var path = expression.GetBindingPath();

        var binding = new Binding(path)
            .SetConverter(converter);

        return target.BindValue(targetProperty, binding, null);
    }

    public static DisposableItem<TAvaloniaObject> BindBindingValue<TAvaloniaObject>(
    this TAvaloniaObject target,
    AvaloniaProperty     targetProperty,
    Expression           expression,
    Action<Binding>      setup)
        where TAvaloniaObject : IAvaloniaObject
    {
        var path    = expression.GetBindingPath();
        var binding = new Binding(path);
        setup.Invoke(binding);

        return target.BindValue(targetProperty, binding);
    }

    public static DisposableItem<TAvaloniaObject> BindBindingValue<TAvaloniaObject>(
    this TAvaloniaObject target,
    AvaloniaProperty     targetProperty,
    Expression           expression,
    object               source)
        where TAvaloniaObject : IAvaloniaObject
    {
        var path = expression.GetBindingPath();

        var binding = new Binding(path)
            .SetSource(source);

        return target.BindValue(targetProperty, binding, null);
    }

    public static DisposableItem<TAvaloniaObject> BindBindingValue<TAvaloniaObject>(
    this TAvaloniaObject target,
    AvaloniaProperty     targetProperty,
    Expression           expression,
    BindingMode          mode)
        where TAvaloniaObject : IAvaloniaObject
    {
        var path    = expression.GetBindingPath();
        var binding = new Binding(path, mode);

        return target.BindValue(targetProperty, binding);
    }

    public static DisposableItem<TAvaloniaObject> BindTemplateValue<TAvaloniaObject>(this TAvaloniaObject target,
        AvaloniaProperty targetProperty,
        AvaloniaProperty property,
        IValueConverter converter)
        where TAvaloniaObject : IAvaloniaObject
    {
        var templateBinding = new TemplateBinding(property)
            .SetConverter(converter);

        return target.BindValue(targetProperty, templateBinding, null);
    }

    public static DisposableItem<TAvaloniaObject> BindTemplateValue<TAvaloniaObject>(this TAvaloniaObject target,
        AvaloniaProperty targetProperty,
        AvaloniaProperty property,
        BindingMode mode)
        where TAvaloniaObject : IAvaloniaObject
    {
        var templateBinding = new TemplateBinding(property)
            .SetMode(mode);

        return target.BindValue(targetProperty, templateBinding, null);
    }

    public static DisposableItem<TAvaloniaObject> BindTemplateValue<TAvaloniaObject>(this TAvaloniaObject target,
        AvaloniaProperty targetProperty,
        AvaloniaProperty property)
        where TAvaloniaObject : IAvaloniaObject
    {
        var templateBinding = new TemplateBinding(property);

        return target.BindValue(targetProperty, templateBinding, null);
    }

    public static DisposableItem<TAvaloniaObject> BindValue<TAvaloniaObject>(this TAvaloniaObject target,
                                                                             AvaloniaProperty     property,
                                                                             IBinding             binding,
                                                                             object?              anchor = null)
        where TAvaloniaObject : IAvaloniaObject
    {
        var disposable = target.Bind(property, binding, anchor);

        return new DisposableItem<TAvaloniaObject>(target, disposable);
    }

    public static DisposableItem<TAvaloniaObject> BindValue<TAvaloniaObject>(this TAvaloniaObject target,
                                                                             AvaloniaProperty     property,
                                                                             IObservable<object>  source,
                                                                             BindingPriority priority =
                                                                                 BindingPriority.LocalValue)
        where TAvaloniaObject : IAvaloniaObject
    {
        var disposable = target.Bind(property, source, priority);

        return new DisposableItem<TAvaloniaObject>(target, disposable);
    }
}