namespace WorkTool.Core.Modules.DependencyInjector.Models;

public class ArgumentValue
{
    public Type    Type  { get; }
    public object? Value { get; }

    public ArgumentValue(Type type, object value)
    {
        Type  = type.ThrowIfNull();
        Value = value;
    }
}

public class ArgumentValue<TType> : ArgumentValue
{
    public ArgumentValue(TType value) : base(typeof(TType), value)
    {
    }
}