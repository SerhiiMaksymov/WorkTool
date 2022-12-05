namespace WorkTool.Core.Modules.AvaloniaUi.Extensions;

public static class StyledElementExtension
{
    public static TStyledElement SetName<TStyledElement>(
        this TStyledElement styledElement,
        string name,
        INameScope nameScope
    ) where TStyledElement : StyledElement
    {
        return styledElement.SetName(name).RegisterInNameScope(nameScope);
    }
}
