namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class BindingExtension
{
    public static TBinding SetRelativeSource<TBinding>(this TBinding binding, RelativeSourceMode mode)
        where TBinding : Binding
    {
        var relativeSource = new RelativeSource(mode);

        return binding.SetRelativeSource(relativeSource);
    }
}