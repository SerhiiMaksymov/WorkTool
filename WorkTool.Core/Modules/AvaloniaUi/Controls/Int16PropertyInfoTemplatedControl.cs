namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class Int16PropertyInfoTemplatedControl : PropertyInfoTemplatedControl<short, NumericUpDown>
{
    static Int16PropertyInfoTemplatedControl()
    {
        IObjectValue.ObjectProperty.AddOwner<Int16PropertyInfoTemplatedControl>(x => x.Object);
    }

    public Int16PropertyInfoTemplatedControl()
        : base(
            (property, _, control, _) =>
            {
                control
                    .GetObservable(NumericUpDown.ValueProperty)
                    .Subscribe(x => property.Value = x.HasValue ? (short)x.Value : (short)0);

                property.GetObservable(ValueProperty).Subscribe(x => control.Value = x);
            }
        ) { }
}
