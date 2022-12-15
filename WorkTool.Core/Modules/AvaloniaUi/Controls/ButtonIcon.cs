namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class ButtonIcon : Button
{
    public static readonly AvaloniaProperty<Geometry> DataProperty = AvaloniaProperty.Register<
        ButtonIcon,
        Geometry
    >(nameof(Data));

    public Geometry Data
    {
        get => GetValue(DataProperty).ThrowIfNull().ThrowIfIsNot<Geometry>();
        set => SetValue(DataProperty, value);
    }
}
