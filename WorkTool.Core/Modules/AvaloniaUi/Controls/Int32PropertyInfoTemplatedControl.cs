namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class Int32PropertyInfoTemplatedControl : PropertyInfoTemplatedControl<int, NumericUpDown>
{
    static Int32PropertyInfoTemplatedControl()
    {
        ObjectProperty.AddOwner<Int32PropertyInfoTemplatedControl>(x => x.Object);
    }

    public Int32PropertyInfoTemplatedControl()
        : base(
            (property, _, control, _) =>
            {
                control
                    .GetObservable(NumericUpDown.ValueProperty)
                    .Subscribe(x => property.Value = (int)x);

                property.GetObservable(ValueProperty).Subscribe(x => control.Value = x);
            }
        ) { }
}
