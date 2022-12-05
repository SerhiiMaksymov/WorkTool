namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class NullablePropertyInfoTemplatedControl<TValue, TControl>
    : PropertyInfoTemplatedControl<TValue, TControl>
    where TControl : class, IAvaloniaObject, new()
    where TValue : class
{
    public static readonly DirectProperty<
        NullablePropertyInfoTemplatedControl<TValue, TControl>,
        bool
    > IsNullProperty = AvaloniaProperty.RegisterDirect<
        NullablePropertyInfoTemplatedControl<TValue, TControl>,
        bool
    >(nameof(IsNull), o => o.IsNull, (o, v) => o.IsNull = v);
    private bool isNull;

    public bool IsNull
    {
        get => isNull;
        set => SetAndRaise(IsNullProperty, ref isNull, value);
    }

    static NullablePropertyInfoTemplatedControl()
    {
        IsNullProperty.Changed.AddClassHandler<
            NullablePropertyInfoTemplatedControl<TValue, TControl>
        >((_, e) => IsNullChanged(e));

        IObjectValue.ObjectProperty.AddOwner<
            NullablePropertyInfoTemplatedControl<TValue, TControl>
        >(x => x.Object);
    }

    public NullablePropertyInfoTemplatedControl(
        Action<
            PropertyInfoTemplatedControl<TValue, TControl>,
            TemplateAppliedEventArgs,
            TControl,
            TextBlock
        > onApplyTemplate
    ) : base(onApplyTemplate) { }

    private static void IsNullChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var sender = (NullablePropertyInfoTemplatedControl<TValue, TControl>)e.Sender;

        if (sender.IsNull)
        {
            sender.Value = null;
        }
    }
}
