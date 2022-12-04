namespace WorkTool.Core.Modules.AvaloniaUi.Interfaces;

public interface ICollectionChangedListener
{
    void PreChanged(INotifyCollectionChanged  sender, NotifyCollectionChangedEventArgs e);
    void Changed(INotifyCollectionChanged     sender, NotifyCollectionChangedEventArgs e);
    void PostChanged(INotifyCollectionChanged sender, NotifyCollectionChangedEventArgs e);
}