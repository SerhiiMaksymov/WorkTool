namespace WorkTool.Core.Modules.Ui.Interfaces;

public interface IContentView
{
    void SetContent(Func<object> content);
}
