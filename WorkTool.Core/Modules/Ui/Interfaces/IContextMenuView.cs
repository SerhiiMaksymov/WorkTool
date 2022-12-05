namespace WorkTool.Core.Modules.Ui.Interfaces;

public interface IContextMenuView
{
    void AddMenuItem(TreeNode<string, MenuItemContext> node);
}
