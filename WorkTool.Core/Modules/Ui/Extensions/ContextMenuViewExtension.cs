namespace WorkTool.Core.Modules.Ui.Extensions;

public static class ContextMenuViewExtension
{
    public static TContextMenuView AddMenuItemValue<TContextMenuView>(
    this TContextMenuView             contextMenuView,
    TreeNode<string, MenuItemContext> node)
        where TContextMenuView : IContextMenuView
    {
        contextMenuView.AddMenuItem(node);

        return contextMenuView;
    }
}