namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class KeyboardKeyGestureExtension
{
    public static KeyGesture ToKeyGesture(this KeyboardKeyGesture keyboardKeyGesture)
    {
        var key = keyboardKeyGesture.Key.ToKey();
        var keyModifiers = keyboardKeyGesture.KeyModifiers.ToKeyModifiers();

        return new KeyGesture(key, keyModifiers);
    }
}
