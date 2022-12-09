namespace WorkTool.Core.Modules.SmsClub.ViewModels;

public class ControlPanelViewModel : ViewModelBase
{
    private readonly ISmsClubSender<object> smsClubSender;

    public ControlPanelViewModel(
        IHumanizing<Exception, object> humanizing,
        IMessageBoxView messageBoxView,
        ISmsClubSender<object> smsClubSender
    ) : base(humanizing, messageBoxView)
    {
        this.smsClubSender = smsClubSender;
        Originators = new AvaloniaList<string>();
        LoadedCommand = CreateCommand(UpdateOriginatorsAsync);
    }

    public AvaloniaList<string> Originators { get; }
    public ICommand LoadedCommand { get; }

    private async Task UpdateOriginatorsAsync()
    {
        var originators = await smsClubSender.GetOriginatorsAsync();
        Originators.AddRange(originators.SuccessRequest.ThrowIfNull().Info.ThrowIfNull());
    }
}
