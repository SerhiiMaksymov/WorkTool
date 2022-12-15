namespace WorkTool.Core.Modules.DependencyInjection.Models;

public class ArgumentValue
{
    public Type Type { get; }
    public object Value { get; }

    public ArgumentValue(Type type, object value)
    {
        Type = type.ThrowIfNull();
        Value = value;
    }
}

public class ArgumentValue<TType> : ArgumentValue where TType : notnull
{
    public ArgumentValue(TType value) : base(typeof(TType), value) { }
}
