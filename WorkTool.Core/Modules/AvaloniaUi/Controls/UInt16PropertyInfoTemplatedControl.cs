namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class UInt16PropertyInfoTemplatedControl
    : PropertyInfoTemplatedControl<ushort, NumericUpDown>
{
    static UInt16PropertyInfoTemplatedControl()
    {
        ObjectProperty.AddOwner<UInt16PropertyInfoTemplatedControl>(x => x.Object);
        TitleProperty.AddOwner<UInt16PropertyInfoTemplatedControl>(x => x.Title);
    }

    public UInt16PropertyInfoTemplatedControl()
        : base(
            (property, _, control, _) =>
            {
                control
                    .GetObservable(NumericUpDown.ValueProperty)
                    .Subscribe(x => property.Value = (ushort)x);

                property.GetObservable(ValueProperty).Subscribe(x => control.Value = x);
            }
        ) { }
}
