namespace WorkTool.Core.Modules.MaterialDesign.Controls;

public class ButtonMaterialIcon : Button
{
    public static readonly AvaloniaProperty<MaterialIconKind> KindProperty =
        AvaloniaProperty.Register<ButtonMaterialIcon, MaterialIconKind>(nameof(Kind));

    public MaterialIconKind Kind
    {
        get => GetValue(KindProperty).ThrowIfNull().ThrowIfIsNot<MaterialIconKind>();
        set => SetValue(KindProperty, value);
    }
}
