namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class DynamicResourceBinding : IBinding
{
    private object? _anchor;
    private BindingPriority _priority;

    public object? ResourceKey { get; set; }

    public DynamicResourceBinding() { }

    public DynamicResourceBinding(object resourceKey)
    {
        ResourceKey = resourceKey;
    }

    InstancedBinding? IBinding.Initiate(
        IAvaloniaObject target,
        AvaloniaProperty? targetProperty,
        object? anchor,
        bool enableDataValidation
    )
    {
        if (ResourceKey is null)
        {
            return null;
        }

        var control = target as IResourceHost ?? _anchor as IResourceHost;

        if (control != null)
        {
            var source = control.GetResourceObservable(ResourceKey, GetConverter(targetProperty));

            return InstancedBinding.OneWay(source, _priority);
        }

        if (_anchor is IResourceProvider resourceProvider)
        {
            var source = resourceProvider.GetResourceObservable(
                ResourceKey,
                GetConverter(targetProperty)
            );

            return InstancedBinding.OneWay(source, _priority);
        }

        return null;
    }

    private Func<object?, object?>? GetConverter(AvaloniaProperty? targetProperty)
    {
        if (targetProperty?.PropertyType == typeof(IBrush))
        {
            return x => ColorToBrushConverter.Convert(x, typeof(IBrush));
        }

        return null;
    }
}
