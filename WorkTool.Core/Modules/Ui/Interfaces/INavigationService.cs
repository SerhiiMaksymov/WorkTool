namespace WorkTool.Core.Modules.Ui.Interfaces;

public interface INavigationService<in T> where T : notnull
{
    void Navigate(T value);
    void NavigateBack();
}
