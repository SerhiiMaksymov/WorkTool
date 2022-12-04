namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class BooleanPropertyInfoTemplatedControl : PropertyInfoTemplatedControl<bool, CheckBox>
{
    static BooleanPropertyInfoTemplatedControl()
    {
        IObjectValue.ObjectProperty.AddOwner<BooleanPropertyInfoTemplatedControl>(x => x.Object);
    }

    public BooleanPropertyInfoTemplatedControl()
        : base(
            (property, _, control, _) =>
            {
                control.GetObservable(ToggleButton.IsCheckedProperty)
                    .Subscribe(x => property.Value = x.HasValue ? x.Value : false);

                property.GetObservable(ValueProperty)
                    .Subscribe(x => control.IsChecked = x);
            })
    {
    }
}