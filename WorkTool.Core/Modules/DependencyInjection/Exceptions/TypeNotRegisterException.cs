namespace WorkTool.Core.Modules.DependencyInjection.Exceptions;

public class TypeNotRegisterException : Exception
{
    public TypeNotRegisterException(Type type) : base($"Type {type} not register.")
    {
        Type = type;
    }

    public Type Type { get; }
}
