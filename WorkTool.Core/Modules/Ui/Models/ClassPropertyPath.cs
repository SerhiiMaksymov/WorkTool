namespace WorkTool.Core.Modules.Ui.Models;

public record ClassPropertyPath
{
    public Type         Type         { get; }
    public PropertyInfo PropertyInfo { get; }

    public ClassPropertyPath(Type type, PropertyInfo propertyInfo)
    {
        Type         = type.ThrowIfNull();
        PropertyInfo = propertyInfo.ThrowIfNull();
    }
}