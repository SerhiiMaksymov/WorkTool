namespace WorkTool.Core.Modules.FileSystem.ViewModel;

public class DiskUsageViewModel : ViewModelBase
{
    public DiskUsageViewModel(
        IHumanizing<Exception, object> humanizing,
        IMessageBoxView messageBoxView
    ) : base(humanizing, messageBoxView) { }
}
