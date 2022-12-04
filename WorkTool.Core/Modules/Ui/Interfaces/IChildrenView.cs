namespace WorkTool.Core.Modules.Ui.Interfaces;

public interface IChildrenView
{
    void AddChild(Func<object> item);
}