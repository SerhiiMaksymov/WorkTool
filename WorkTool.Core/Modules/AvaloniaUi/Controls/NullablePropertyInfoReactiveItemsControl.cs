namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class NullablePropertyInfoReactiveItemsControl : PropertyInfoReactiveItemsControl
{
    public static readonly DirectProperty<
        NullablePropertyInfoReactiveItemsControl,
        bool
    > IsNullProperty = AvaloniaProperty.RegisterDirect<
        NullablePropertyInfoReactiveItemsControl,
        bool
    >(nameof(IsNull), o => o.IsNull, (o, v) => o.IsNull = v);
    private object? cache;
    private bool isNull;

    public NullablePropertyInfoReactiveItemsControl()
    {
        CheckBox = new CheckBox();
    }

    public CheckBox CheckBox { get; }

    public bool IsNull
    {
        get => isNull;
        set => SetAndRaise(IsNullProperty, ref isNull, value);
    }

    static NullablePropertyInfoReactiveItemsControl()
    {
        ItemsPanelProperty.OverrideDefaultValue<NullablePropertyInfoReactiveItemsControl>(
            new FuncTemplate<Panel>(() => new ItemsRepeater())
        );

        IsNullProperty.Changed.AddClassHandler<NullablePropertyInfoReactiveItemsControl>(
            (_, e) =>
            {
                var sender = (NullablePropertyInfoReactiveItemsControl)e.Sender;

                if (e.NewValue is not bool value)
                {
                    sender.Value = sender.cache;

                    return;
                }

                sender.Value = value ? null : sender.cache;
            }
        );

        ValueProperty.Changed.AddClassHandler<NullablePropertyInfoReactiveItemsControl>(
            (_, e) =>
            {
                var sender = (NullablePropertyInfoReactiveItemsControl)e.Sender;

                if (sender.Value is null)
                {
                    return;
                }

                sender.cache = sender.Value;
            }
        );
    }
}
