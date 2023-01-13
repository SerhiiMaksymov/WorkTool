namespace WorkTool.Core.Modules.ReactiveUI.Services;

public class RoutedViewHostNavigationService<T> : INavigationService<T> where T : notnull
{
    private readonly RoutedViewHost routedViewHost;

    public RoutedViewHostNavigationService(RoutedViewHost routedViewHost)
    {
        this.routedViewHost = routedViewHost;
    }

    public void Navigate(T value)
    {
        var router = routedViewHost.Router.ThrowIfNull();
        var routableViewModel = value.ThrowIfIsNot<IRoutableViewModel>();
        router.Navigate.Execute(routableViewModel);
    }

    public void NavigateBack()
    {
        var router = routedViewHost.Router.ThrowIfNull();
        router.NavigateBack.Execute();
    }
}
