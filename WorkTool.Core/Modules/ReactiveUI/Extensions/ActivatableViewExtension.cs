namespace WorkTool.Core.Modules.ReactiveUI.Extensions;

public static class ActivatableViewExtension
{
    public static DisposableItem<TActivatableView> WhenActivated<TActivatableView>(this TActivatableView item,
        Action<CompositeDisposable, TActivatableView>                                                    block,
        IViewFor?                                                                                        view = null)
        where TActivatableView : IActivatableView
    {
        var disposable = item.WhenActivated(disposables => block.Invoke(disposables, item), view);

        return new DisposableItem<TActivatableView>(item, disposable);
    }
}