namespace WorkTool.Core.Modules.Common.Exceptions;

public class NegativeException<TValue> : Exception
{
    public string Name  { get; }
    public TValue Value { get; }

    public NegativeException(string name, TValue value) : base($"{name}({value}) can't be negative.")
    {
        Name  = name;
        Value = value;
    }
}