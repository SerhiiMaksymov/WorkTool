namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class NullablePropertyInfoReactiveItemsControl : PropertyInfoReactiveItemsControl
{
    public static readonly DirectProperty<NullablePropertyInfoReactiveItemsControl, bool> IsNullProperty =
        AvaloniaProperty.RegisterDirect<NullablePropertyInfoReactiveItemsControl, bool>(
            nameof(IsNull),
            o => o.IsNull,
            (o, v) => o.IsNull = v);
    private object cache;
    private bool   isNull;

    public CheckBox CheckBox { get; }

    public bool IsNull
    {
        get => isNull;
        set => SetAndRaise(IsNullProperty, ref isNull, value);
    }

    static NullablePropertyInfoReactiveItemsControl()
    {
        ItemsPanelProperty.OverrideDefaultValue<NullablePropertyInfoReactiveItemsControl>(
            new FuncTemplate<IPanel>(() => new ItemsRepeater()));

        IsNullProperty.Changed.AddClassHandler<NullablePropertyInfoReactiveItemsControl>(
            (_, e) =>
            {
                var sender = (NullablePropertyInfoReactiveItemsControl)e.Sender;

                if ((bool)e.NewValue)
                {
                    sender.Value = null;
                }
                else
                {
                    sender.Value = sender.cache;
                }
            });

        ValueProperty.Changed.AddClassHandler<NullablePropertyInfoReactiveItemsControl>(
            (_, e) =>
            {
                var sender = (NullablePropertyInfoReactiveItemsControl)e.Sender;

                if (sender.Value is null)
                {
                    return;
                }

                sender.cache = sender.Value;
            });
    }

    public NullablePropertyInfoReactiveItemsControl()
    {
        CheckBox = new CheckBox();
    }
}