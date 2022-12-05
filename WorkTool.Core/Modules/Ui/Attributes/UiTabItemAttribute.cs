namespace WorkTool.Core.Modules.Ui.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class UiTabItemAttribute : Attribute
{
    public Type TabControlViewType { get; }
    public string Header { get; }
    public Type ContentType { get; }

    public UiTabItemAttribute(Type tabControlViewType, string header, Type contentType)
    {
        TabControlViewType = tabControlViewType;
        Header = header;
        ContentType = contentType;
    }
}
