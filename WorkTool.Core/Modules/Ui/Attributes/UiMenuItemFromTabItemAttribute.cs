namespace WorkTool.Core.Modules.Ui.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public class UiMenuItemFromTabItemAttribute : Attribute
{
    public Type MenuViewType { get; }
    public string[] Path { get; }
    public Type TabControlViewType { get; }
    public Type ContentType { get; }

    public UiMenuItemFromTabItemAttribute(
        Type menuView,
        Type tabControlViewType,
        Type contentType,
        params string[] path
    )
    {
        MenuViewType = menuView.ThrowIfNull();
        path.ThrowIfNullOrEmpty();
        Path = path;
        TabControlViewType = tabControlViewType.ThrowIfNull();
        ContentType = contentType.ThrowIfNull();
    }
}
