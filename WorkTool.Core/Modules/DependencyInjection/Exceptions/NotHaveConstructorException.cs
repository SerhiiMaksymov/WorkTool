namespace WorkTool.Core.Modules.DependencyInjection.Exceptions;

public class NotHaveConstructorException : Exception
{
    public NotHaveConstructorException(Type type) : base($"Type {type} not have constructor.")
    {
        Type = type;
    }

    public Type Type { get; }
}
