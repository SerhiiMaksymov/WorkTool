namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class InputElementExtension
{
    public static TInputElement SetCursor<TInputElement>(
        this TInputElement inputElement,
        StandardCursorType type
    ) where TInputElement : InputElement
    {
        var cursor = new Cursor(type);

        return inputElement.SetCursor(cursor);
    }
}
