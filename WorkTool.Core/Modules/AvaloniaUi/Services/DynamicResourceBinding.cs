namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class DynamicResourceBinding : IBinding
{
    private object? anchor;
    private BindingPriority priority;

    public object? ResourceKey { get; set; }

    public DynamicResourceBinding()
    {
        anchor = null;
        priority = BindingPriority.Animation;
    }

    public DynamicResourceBinding(object resourceKey)
    {
        ResourceKey = resourceKey;
    }

    InstancedBinding? IBinding.Initiate(
        IAvaloniaObject target,
        AvaloniaProperty? targetProperty,
        object? newAnchor,
        bool enableDataValidation
    )
    {
        if (ResourceKey is null)
        {
            return null;
        }

        var control = target as IResourceHost ?? this.anchor as IResourceHost;

        if (control != null)
        {
            var source = control.GetResourceObservable(ResourceKey, GetConverter(targetProperty));

            return InstancedBinding.OneWay(source, priority);
        }

        if (this.anchor is IResourceProvider resourceProvider)
        {
            var source = resourceProvider.GetResourceObservable(
                ResourceKey,
                GetConverter(targetProperty)
            );

            return InstancedBinding.OneWay(source, priority);
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
