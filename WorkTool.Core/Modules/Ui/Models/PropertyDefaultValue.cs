namespace WorkTool.Core.Modules.Ui.Models;

public class PropertyDefaultValue
{
    public string Name { get; }
    public object Value { get; }

    public PropertyDefaultValue(string name, object value)
    {
        Name = name.ThrowIfNullOrWhiteSpace();
        Value = value.ThrowIfNull();
    }
}
