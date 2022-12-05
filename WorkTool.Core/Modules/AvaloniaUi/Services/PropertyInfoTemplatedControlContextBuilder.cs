namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class PropertyInfoItemsControlContextBuilder : IBuilder<PropertyInfoTemplatedControlContext>
{
    private readonly Dictionary<Type, Func<object, PropertyInfo, IObjectValue>> typeMatchs;
    private Func<object, PropertyInfo, IObjectValue> defaultView;

    public PropertyInfoItemsControlContextBuilder()
    {
        typeMatchs = new Dictionary<Type, Func<object, PropertyInfo, IObjectValue>>();
    }

    public PropertyInfoTemplatedControlContext Build()
    {
        return new PropertyInfoTemplatedControlContext(typeMatchs, defaultView);
    }

    public PropertyInfoItemsControlContextBuilder SetDefaultView(
        Func<object, PropertyInfo, IObjectValue> match
    )
    {
        defaultView = match;

        return this;
    }

    public PropertyInfoItemsControlContextBuilder SetView(
        Type type,
        Func<object, PropertyInfo, IObjectValue> match
    )
    {
        typeMatchs[type] = match;

        return this;
    }

    public PropertyInfoItemsControlContextBuilder SetView<TType>(
        Func<object, PropertyInfo, IObjectValue> match
    )
    {
        return SetView(typeof(TType), match);
    }

    public static PropertyInfoItemsControlContextBuilder CreateDefaultBuilder(
        IResolver resolver,
        UiContext uiContext
    )
    {
        var result = new PropertyInfoItemsControlContextBuilder()
            .SetView<string>(
                (obj, property) =>
                    new StringNullablePropertyInfoTemplatedControl(
                        SetupContextMenu<string>(uiContext)
                    )
                    {
                        Value = (string)property.GetValue(obj),
                        Object = obj,
                        PropertyInfo = property,
                        Title = $"{property.Name}[{property.PropertyType}]"
                    }
            )
            .SetView<short>(
                (obj, property) =>
                    new Int16PropertyInfoTemplatedControl
                    {
                        Value = (short)property.GetValue(obj),
                        Object = obj,
                        PropertyInfo = property,
                        Title = $"{property.Name}[{property.PropertyType}]"
                    }
            )
            .SetView<ushort>(
                (obj, property) =>
                    new UInt16PropertyInfoTemplatedControl
                    {
                        Value = (ushort)property.GetValue(obj),
                        Object = obj,
                        PropertyInfo = property,
                        Title = $"{property.Name}[{property.PropertyType}]"
                    }
            )
            .SetView<uint>(
                (obj, property) =>
                    new UInt32PropertyInfoTemplatedControl
                    {
                        Value = (uint)property.GetValue(obj),
                        Object = obj,
                        PropertyInfo = property,
                        Title = $"{property.Name}[{property.PropertyType}]"
                    }
            )
            .SetView<ulong>(
                (obj, property) =>
                    new UInt64PropertyInfoTemplatedControl
                    {
                        Value = (ulong)property.GetValue(obj),
                        Object = obj,
                        PropertyInfo = property,
                        Title = $"{property.Name}[{property.PropertyType}]"
                    }
            )
            .SetView<long>(
                (obj, property) =>
                    new Int64PropertyInfoTemplatedControl
                    {
                        Value = (long)property.GetValue(obj),
                        Object = obj,
                        PropertyInfo = property,
                        Title = $"{property.Name}[{property.PropertyType}]"
                    }
            )
            .SetView<int>(
                (obj, property) =>
                    new Int32PropertyInfoTemplatedControl
                    {
                        Value = (int)property.GetValue(obj),
                        Object = obj,
                        PropertyInfo = property,
                        Title = $"{property.Name}[{property.PropertyType}]"
                    }
            )
            .SetView<byte>(
                (obj, property) =>
                    new BytePropertyInfoTemplatedControl
                    {
                        Value = (byte)property.GetValue(obj),
                        Object = obj,
                        PropertyInfo = property,
                        Title = $"{property.Name}[{property.PropertyType}]"
                    }
            )
            .SetView<sbyte>(
                (obj, property) =>
                    new SBytePropertyInfoTemplatedControl
                    {
                        Value = (sbyte)property.GetValue(obj),
                        Object = obj,
                        PropertyInfo = property,
                        Title = $"{property.Name}[{property.PropertyType}]"
                    }
            )
            .SetView<bool>(
                (obj, property) =>
                    new BooleanPropertyInfoTemplatedControl
                    {
                        Value = (bool)property.GetValue(obj),
                        Object = obj,
                        PropertyInfo = property,
                        Title = $"{property.Name}[{property.PropertyType}]"
                    }
            )
            .SetView<double>(
                (obj, property) =>
                    new DoublePropertyInfoTemplatedControl
                    {
                        Value = (double)property.GetValue(obj),
                        Object = obj,
                        PropertyInfo = property,
                        Title = $"{property.Name}[{property.PropertyType}]"
                    }
            )
            .SetView<decimal>(
                (obj, property) =>
                    new DecimalPropertyInfoTemplatedControl
                    {
                        Value = (decimal)property.GetValue(obj),
                        Object = obj,
                        PropertyInfo = property,
                        Title = $"{property.Name}[{property.PropertyType}]"
                    }
            )
            .SetDefaultView(
                (obj, property) =>
                {
                    if (obj.GetType().IsValueType)
                    {
                        var propertyInfoItemsControl =
                            resolver.Resolve<PropertyInfoReactiveItemsView>();
                        propertyInfoItemsControl.Value = resolver.Resolve(property.PropertyType);
                        propertyInfoItemsControl.Object = obj;
                        propertyInfoItemsControl.PropertyInfo = property;
                        propertyInfoItemsControl.Title =
                            $"{property.Name}[{property.PropertyType}]";

                        return propertyInfoItemsControl;
                    }

                    var nullablePropertyInfoItemsControl =
                        resolver.Resolve<NullablePropertyInfoReactiveItemsView>();
                    nullablePropertyInfoItemsControl.Value = resolver.Resolve(
                        property.PropertyType
                    );
                    nullablePropertyInfoItemsControl.Object = obj;
                    nullablePropertyInfoItemsControl.PropertyInfo = property;
                    nullablePropertyInfoItemsControl.Title =
                        $"{property.Name}[{property.PropertyType}]";

                    return nullablePropertyInfoItemsControl;
                }
            );

        return result;
    }

    private static Action<
        PropertyInfoTemplatedControl<TValue, TextBox>,
        TemplateAppliedEventArgs,
        TextBox,
        TextBlock
    > SetupContextMenu<TValue>(UiContext uiContext)
    {
        return (property, _, control, _) =>
            property
                .GetObservable(PropertyInfoTemplatedControl<TValue, TextBox>.PropertyInfoProperty)
                .Subscribe(_ =>
                {
                    var defaultValues = uiContext.GetDefaultValues(
                        property.Object.GetType(),
                        property.PropertyInfo
                    );

                    if (defaultValues.IsEmpty())
                    {
                        return;
                    }

                    var menuFlyout = new MenuFlyout().AddItems(control.CreateDefaultMenuItems());

                    if (control.ContextFlyout is MenuFlyout controlContextFlyout)
                    {
                        control.ContextFlyout = menuFlyout;
                    }

                    menuFlyout.AddItems(
                        defaultValues.Select(
                            x =>
                                new MenuItem
                                {
                                    Command = ReactiveCommand.Create(
                                        () => property.Value = (TValue)x.Value
                                    ),
                                    Header = x.Name
                                }
                        )
                    );
                });
    }
}
