namespace WorkTool.Core.Modules.Common.Exceptions;

public class ZeroException : Exception
{
    public string Name { get; }

    public ZeroException(string name) : base($"{name} can't be zero.")
    {
        Name = name;
    }
}