namespace WorkTool.Core.Modules.AvaloniaUi.Interfaces;

public interface IObjectValue : IAvaloniaObject
{
    public static readonly DirectProperty<IObjectValue, object?> ObjectProperty =
        AvaloniaProperty.RegisterDirect<IObjectValue, object?>(
            nameof(Object),
            o => o.Object,
            (o, v) => o.Object = v
        );

    object? Object { get; set; }
}
