namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class Int64PropertyInfoTemplatedControl : PropertyInfoTemplatedControl<long, NumericUpDown>
{
    static Int64PropertyInfoTemplatedControl()
    {
        IObjectValue.ObjectProperty.AddOwner<Int64PropertyInfoTemplatedControl>(x => x.Object);
    }

    public Int64PropertyInfoTemplatedControl()
        : base(
            (property, _, control, _) =>
            {
                control
                    .GetObservable(NumericUpDown.ValueProperty)
                    .Subscribe(x => property.Value = x.HasValue ? (long)x.Value : 0);

                property.GetObservable(ValueProperty).Subscribe(x => control.Value = x);
            }
        ) { }
}
