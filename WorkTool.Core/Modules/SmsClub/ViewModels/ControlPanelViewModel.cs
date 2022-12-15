namespace WorkTool.Core.Modules.SmsClub.ViewModels;

public class ControlPanelViewModel : ViewModelBase
{
    private readonly ISmsClubSender<object> smsClubSender;
    private double balance;
    private string? currency;
    private string? smsIds;
    private string? info;

    public ControlPanelViewModel(
        IHumanizing<Exception, object> humanizing,
        IMessageBoxView messageBoxView,
        ISmsClubSender<object> smsClubSender
    ) : base(humanizing, messageBoxView)
    {
        this.smsClubSender = smsClubSender;
        Originators = new AvaloniaList<string>();
        UpdateBalanceCommand = CreateCommand(UpdateBalanceAsync);
        UpdateOriginatorsCommand = CreateCommand(UpdateOriginatorsAsync);
        GetSmsesStatusCommand = CreateCommand(GetSmsesStatusAsync);
        LoadedCommand = CreateCommand(RefreshAsync);
        RefreshCommand = CreateCommand(RefreshAsync);
    }

    public double Balance
    {
        get => balance;
        set => this.RaiseAndSetIfChanged(ref balance, value);
    }

    public string? Currency
    {
        get => currency;
        set => this.RaiseAndSetIfChanged(ref currency, value);
    }

    public string? SmsIds
    {
        get => smsIds;
        set => this.RaiseAndSetIfChanged(ref smsIds, value);
    }

    public string? Info
    {
        get => info;
        set => this.RaiseAndSetIfChanged(ref info, value);
    }

    public AvaloniaList<string> Originators { get; }
    public ICommand LoadedCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand UpdateBalanceCommand { get; }
    public ICommand UpdateOriginatorsCommand { get; }
    public ICommand GetSmsesStatusCommand { get; }

    private Task RefreshAsync()
    {
        return Task.WhenAll(UpdateBalanceAsync(), UpdateOriginatorsAsync());
    }

    private async Task GetSmsesStatusAsync()
    {
        var smsIdsString = SmsIds.ThrowIfNullOrWhiteSpace();
        var smsIds = smsIdsString.Split(',').Select(x => x.Trim());
        var status = await smsClubSender.GetSmsStatusAsync(smsIds);

        Info = Info.AddLine(
            status.SuccessRequest
                .ThrowIfNull()
                .Info.ThrowIfNull()
                .Select(x => $"{x.Key}: {x.Value}")
                .JoinString(Environment.NewLine)
        );
    }

    private async Task SendSmsAsync() { }

    private async Task UpdateBalanceAsync()
    {
        var balanceResponse = await smsClubSender.GetBalanceAsync();
        var obj = balanceResponse.SuccessRequest.ThrowIfNull().Object.ThrowIfNull();
        Balance = obj.Money;
        Currency = obj.Currency;
    }

    private async Task UpdateOriginatorsAsync()
    {
        var originators = await smsClubSender.GetOriginatorsAsync();
        Originators.Update(originators.SuccessRequest.ThrowIfNull().Info.ThrowIfNull());
    }
}
