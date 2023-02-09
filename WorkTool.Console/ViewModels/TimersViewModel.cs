using System.Reactive.Concurrency;
using System.Windows.Input;

using AutoMapper;

using Avalonia.Collections;
using Avalonia.Threading;

using ReactiveUI;

using WorkTool.Console.Interfaces;
using WorkTool.Console.Views;
using WorkTool.Core.Modules.Audio.Interfaces;
using WorkTool.Core.Modules.AvaloniaUi.Helpers;
using WorkTool.Core.Modules.Common.Interfaces;
using WorkTool.Core.Modules.DependencyInjection.Interfaces;
using WorkTool.Core.Modules.Ui.Extensions;
using WorkTool.Core.Modules.Ui.Interfaces;

namespace WorkTool.Console.ViewModels;

public class TimersViewModel : ViewModelBase
{
    private readonly IMapper mapper;
    private readonly IUnitOfWork unitOfWork;

    public TimersViewModel(
        IScheduler scheduler,
        IHumanizing<Exception, object> humanizing,
        IMessageBoxView messageBoxView,
        IInvoker invoker,
        IMapper mapper,
        IUnitOfWork unitOfWork
    ) : base(scheduler, humanizing, messageBoxView, invoker)
    {
        this.mapper = mapper;
        this.unitOfWork = unitOfWork;
        Timers = new();
        StartedTimers = new();
        CreateTimerCommand = CreateCommand(CreateTimerAsync);
        DeleteTimerCommand = CreateCommand<TimerItemViewModel>(DeleteTimerAsync);
        InitializedCommand = CreateCommand(LoadTimersAsync);
        StartTimerCommand = CreateCommand<TimerItemViewModel>(StartTimer);
        PauseTimerCommand = CreateCommand<StartedTimerItemViewModel>(PauseTimer);
        DeleteStartedTimerCommand = CreateCommand<StartedTimerItemViewModel>(DeleteStartedTimer);
    }

    public AvaloniaList<TimerItemViewModel> Timers { get; }
    public AvaloniaList<StartedTimerItemViewModel> StartedTimers { get; }
    public ICommand CreateTimerCommand { get; }
    public ICommand DeleteTimerCommand { get; }
    public ICommand DeleteStartedTimerCommand { get; }
    public ICommand InitializedCommand { get; }
    public ICommand StartTimerCommand { get; }
    public ICommand PauseTimerCommand { get; }

    private async Task DeleteStartedTimer(StartedTimerItemViewModel item)
    {
        StartedTimers.Remove(item);
        item.Dispose();
        await Task.Delay(AvaloniaUiHelper.RenderTimeout);
    }

    private void PauseTimer(StartedTimerItemViewModel item)
    {
        item.Dispose();
    }

    private void StartTimer(TimerItemViewModel item)
    {
        var started = new StartedTimerItemViewModel { Start = DateTime.Now, Timer = item };
        started.StartTimer();
        StartedTimers.Add(started);
    }

    private async Task LoadTimersAsync()
    {
        var items = await unitOfWork.TimerRepository.GetItemsAsync();
        Timers.Clear();
        AddTimers(items.Select(x => mapper.Map<TimerItemViewModel>(x)));
    }

    private async Task CreateTimerAsync(CreateTimerView view)
    {
        if (!await MessageBoxView.ShowBooleanAsync("Create timer", view))
        {
            return;
        }

        var viewModel = view.ViewModel.ThrowIfNull();
        viewModel.Name.ThrowIfNullOrWhiteSpace();
        var timerItemViewModel = mapper.Map<TimerItemViewModel>(viewModel);
        var timerItem = mapper.Map<TimerItem>(timerItemViewModel);
        await unitOfWork.TimerRepository.AddAsync(timerItem);
        await unitOfWork.SaveChangesAsync();
        timerItemViewModel.Id = timerItem.Id;
        AddTimer(timerItemViewModel);
    }

    private void AddTimer(TimerItemViewModel item)
    {
        Timers.Add(item);

        item.WhenAnyValue(x => x.Name)
            .Subscribe(async _ =>
            {
                var ti = mapper.Map<TimerItem>(item);
                await unitOfWork.TimerRepository.UpdateAsync(ti);
                await unitOfWork.SaveChangesAsync();
            });
    }

    private void AddTimers(IEnumerable<TimerItemViewModel> items)
    {
        foreach (var item in items)
        {
            AddTimer(item);
        }
    }

    private async Task DeleteTimerAsync(TimerItemViewModel item)
    {
        await unitOfWork.TimerRepository.DeleteByIdAsync(item.Id);
        await unitOfWork.SaveChangesAsync();
        Timers.Remove(item);
    }
}
