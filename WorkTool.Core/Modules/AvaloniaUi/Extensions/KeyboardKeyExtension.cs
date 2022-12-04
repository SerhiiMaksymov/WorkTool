namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class KeyboardKeyExtension
{
    public static Key ToKey(this KeyboardKey keyboardKey)
    {
        return (Key)keyboardKey;
    }
}