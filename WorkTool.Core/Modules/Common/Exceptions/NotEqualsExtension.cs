namespace WorkTool.Core.Modules.Common.Exceptions;

public class NotEqualsExtension<TValue> : Exception
{
    public string Name     { get; }
    public TValue Value    { get; }
    public TValue Expected { get; }

    public NotEqualsExtension(string name, TValue value, TValue expected)
        : base($"{name} equals {value} expected {expected}.")
    {
        Name     = name;
        Value    = value;
        Expected = expected;
    }
}