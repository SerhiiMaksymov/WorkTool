namespace WorkTool.Core.Modules.Common.Exceptions;

public class EmptyEnumerableException : Exception
{
    public EmptyEnumerableException(string enumerableName)
        : base($"{enumerableName} empty enumerable.") { }
}
