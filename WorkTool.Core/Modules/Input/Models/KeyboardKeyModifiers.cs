namespace WorkTool.Core.Modules.Input.Models;

[Flags]
public enum KeyboardKeyModifiers
{
    None    = 0,
    Alt     = 1,
    Control = 2,
    Shift   = 4,
    Meta    = 8
}