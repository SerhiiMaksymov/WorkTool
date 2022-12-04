namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class UInt64PropertyInfoTemplatedControl : PropertyInfoTemplatedControl<ulong, NumericUpDown>
{
    static UInt64PropertyInfoTemplatedControl()
    {
        IObjectValue.ObjectProperty.AddOwner<UInt64PropertyInfoTemplatedControl>(x => x.Object);
    }

    public UInt64PropertyInfoTemplatedControl()
        : base(
            (property, _, control, _) =>
            {
                control.GetObservable(NumericUpDown.ValueProperty)
                    .Subscribe(x => property.Value = (ulong)x);

                property.GetObservable(ValueProperty)
                    .Subscribe(x => control.Value = x);
            })
    {
    }
}