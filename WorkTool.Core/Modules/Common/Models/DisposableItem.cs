namespace WorkTool.Core.Modules.Common.Models;

public class DisposableItem<TItem> : IDisposable
{
    private readonly IDisposable disposable;
    public TItem Item { get; }

    public DisposableItem(TItem item, IDisposable disposable)
    {
        Item = item;
        this.disposable = disposable;
    }

    public void Dispose()
    {
        disposable?.Dispose();
    }
}
