namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class ObjectExtension
{
    public static PropertyInfoReactiveItemsControl CreateReflectionControlPropertyInfoItemsControl(
        this object obj,
        IResolver resolver
    )
    {
        if (obj.GetType().IsValueType)
        {
            var propertyInfoItemsControl = resolver.Resolve<PropertyInfoReactiveItemsView>();
            propertyInfoItemsControl.Value = obj;
            propertyInfoItemsControl.Title = obj.GetType().ToString();

            return propertyInfoItemsControl;
        }

        var nullablePropertyInfoItemsControl =
            resolver.Resolve<NullablePropertyInfoReactiveItemsView>();
        nullablePropertyInfoItemsControl.Value = obj;
        nullablePropertyInfoItemsControl.Title = obj.GetType().ToString();

        return nullablePropertyInfoItemsControl;
    }

    public static PropertyInfoReactiveItemsControl CreateReflectionControlPropertyInfoItemsControl(
        this object obj,
        IResolver resolver,
        bool nullable
    )
    {
        if (!nullable)
        {
            var propertyInfoItemsControl = resolver.Resolve<PropertyInfoReactiveItemsView>();
            propertyInfoItemsControl.Value = obj;
            propertyInfoItemsControl.Title = obj.GetType().ToString();

            return propertyInfoItemsControl;
        }

        var nullablePropertyInfoItemsControl =
            resolver.Resolve<NullablePropertyInfoReactiveItemsView>();
        nullablePropertyInfoItemsControl.Value = obj;
        nullablePropertyInfoItemsControl.Title = obj.GetType().ToString();

        return nullablePropertyInfoItemsControl;
    }
}
