namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class KeyboardKeyModifiersExtension
{
    public static KeyModifiers ToKeyModifiers(this KeyboardKeyModifiers keyboardKeyModifiers)
    {
        return (KeyModifiers)keyboardKeyModifiers;
    }
}
