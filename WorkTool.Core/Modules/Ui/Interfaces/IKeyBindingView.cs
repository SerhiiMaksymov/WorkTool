namespace WorkTool.Core.Modules.Ui.Interfaces;

public interface IKeyBindingView
{
    void AddKeyBinding(KeyboardKeyGesture keyGesture, Delegate command);
}