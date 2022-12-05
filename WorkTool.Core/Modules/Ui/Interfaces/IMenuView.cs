namespace WorkTool.Core.Modules.Ui.Interfaces;

public interface IMenuView
{
    void AddMenuItem(TreeNode<string, MenuItemContext> node);
}
