namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class BytePropertyInfoTemplatedControl : PropertyInfoTemplatedControl<byte, NumericUpDown>
{
    static BytePropertyInfoTemplatedControl()
    {
        ObjectProperty.AddOwner<BytePropertyInfoTemplatedControl>(x => x.Object);
    }

    public BytePropertyInfoTemplatedControl()
        : base(
            (property, _, control, _) =>
            {
                control
                    .GetObservable(NumericUpDown.ValueProperty)
                    .Subscribe(x => property.Value = (byte)x);

                property.GetObservable(ValueProperty).Subscribe(x => control.Value = x);
            }
        ) { }
}
