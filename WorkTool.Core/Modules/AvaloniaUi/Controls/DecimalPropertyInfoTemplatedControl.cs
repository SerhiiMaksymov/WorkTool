namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class DecimalPropertyInfoTemplatedControl : PropertyInfoTemplatedControl<decimal, NumericUpDown>
{
    static DecimalPropertyInfoTemplatedControl()
    {
        IObjectValue.ObjectProperty.AddOwner<DecimalPropertyInfoTemplatedControl>(x => x.Object);
    }

    public DecimalPropertyInfoTemplatedControl()
        : base(
            (property, _, control, _) =>
            {
                control.GetObservable(NumericUpDown.ValueProperty)
                    .Subscribe(x => property.Value = x ?? 0);

                property.GetObservable(ValueProperty)
                    .Subscribe(x => control.Value = x);
            })
    {
    }
}