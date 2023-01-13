namespace WorkTool.Core.Modules.AvaloniaUi.Exceptions;

public class NotFoundNamedControlException : Exception
{
    public NotFoundNamedControlException(string controlName, Type controlType)
        : base($"Can't find control {controlType} with name {controlName}.")
    {
        ControlName = controlName;
        ControlType = controlType;
    }

    public string ControlName { get; }
    public Type ControlType { get; }
}
