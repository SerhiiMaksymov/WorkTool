namespace WorkTool.Core.Modules.DependencyInjection.Exceptions;

public class RecursionTypeInvokeException : Exception
{
    public RecursionTypeInvokeException(Type type, Delegate del)
        : base($"{type} contains in parameters {del}.")
    {
        Type = type;
        Delegate = del;
    }

    public Type Type { get; }
    public Delegate Delegate { get; }
}
