namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class TextBoxExtension
{
    public static IEnumerable<MenuItem> CreateDefaultMenuItems<TTextBox>(this TTextBox textBox)
        where TTextBox : TextBox
    {
        yield return new MenuItem()
            .SetHeader("Cut")
            .SetCommand(ReactiveCommand.Create(textBox.Cut))
            .SetInputGesture(TextBox.CutGesture)
            .BindBindingValue(InputElement.IsEnabledProperty, () => textBox.CanCut, textBox)
            .Item;

        yield return new MenuItem()
            .SetHeader("Copy")
            .SetCommand(ReactiveCommand.Create(textBox.Copy))
            .SetInputGesture(TextBox.CopyGesture)
            .BindBindingValue(InputElement.IsEnabledProperty, () => textBox.CanCopy, textBox)
            .Item;

        yield return new MenuItem()
            .SetHeader("Paste")
            .SetCommand(ReactiveCommand.Create(textBox.Paste))
            .SetInputGesture(TextBox.PasteGesture)
            .BindBindingValue(InputElement.IsEnabledProperty, () => textBox.CanPaste, textBox)
            .Item;
    }
}
