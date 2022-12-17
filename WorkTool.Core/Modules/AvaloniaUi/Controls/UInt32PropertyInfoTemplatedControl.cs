namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class UInt32PropertyInfoTemplatedControl : PropertyInfoTemplatedControl<uint, NumericUpDown>
{
    static UInt32PropertyInfoTemplatedControl()
    {
        IObjectValue.ObjectProperty.AddOwner<UInt32PropertyInfoTemplatedControl>(x => x.Object);
    }

    public UInt32PropertyInfoTemplatedControl()
        : base(
            (property, _, control, _) =>
            {
                control
                    .GetObservable(NumericUpDown.ValueProperty)
                    .Subscribe(x => property.Value = (uint)x);

                property.GetObservable(ValueProperty).Subscribe(x => control.Value = x);
            }
        ) { }
}
