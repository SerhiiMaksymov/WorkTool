namespace WorkTool.Core.Modules.Ui.Models;

public class KeyboardKeyBinding
{
    public Delegate Command { get; }
    public IEnumerable<KeyboardKeyGesture> KeyboardKeyGestures { get; }

    public KeyboardKeyBinding(Delegate command, IEnumerable<KeyboardKeyGesture> keyboardKeyGestures)
    {
        Command = command.ThrowIfNull();
        KeyboardKeyGestures = keyboardKeyGestures.ThrowIfNullOrEmpty().ToArray();
    }
}
