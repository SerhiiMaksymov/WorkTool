namespace WorkTool.Core.Modules.Common.Exceptions;

public class WhiteSpaceException : Exception
{
    public string Name { get; }

    public WhiteSpaceException(string name) : base($"{name} can't be white space.")
    {
        Name = name;
    }
}
