namespace WorkTool.Core.Applications.DailyGymnastics.ViewModels;

public class DailyGymnasticsViewModel : ViewModelBase
{
    private static readonly string[] _opetators =
    {
        "*",
        "+",
        "-",
        "/"
    };
    private readonly IRandomArrayItem<string>             _randomArrayItem;
    private readonly IRandom<int>                         _randomInt32;
    private          ulong                                _count;
    private          DailyGymnasticsTab                   _currentTab;
    private          int                                  _firstOperator;
    private          string                               _operation;
    private          int                                  _result;
    private          string                               _sample;
    private          int                                  _secondOperator;
    private          DailyGymnasticsStatistic             _statistic;
    private readonly Stopwatch                            _stopwatch;
    private readonly ObservableAsPropertyHelper<TimeSpan> _time;

    public ICommand StopCommand  { get; }
    public ICommand CheckCommand { get; }
    public ICommand StartCommand { get; }
    public TimeSpan Time         => _time.Value;

    public DailyGymnasticsStatistic Statistic
    {
        get => _statistic;
        set => this.RaiseAndSetIfChanged(ref _statistic, value);
    }

    public DailyGymnasticsTab CurrentTab
    {
        get => _currentTab;
        set => this.RaiseAndSetIfChanged(ref _currentTab, value);
    }

    public ulong Count
    {
        get => _count;
        set => this.RaiseAndSetIfChanged(ref _count, value);
    }

    public string Sample
    {
        get => _sample;
        set => this.RaiseAndSetIfChanged(ref _sample, value);
    }

    public int Result
    {
        get => _result;
        set => this.RaiseAndSetIfChanged(ref _result, value);
    }

    public DailyGymnasticsViewModel(
    IHumanizing<Exception, object> humanizing,
    IMessageBoxView                messageBoxView,
    IRandomArrayItem<string>       randomArrayItem,
    IRandom<int>                   randomInt32) : base(humanizing, messageBoxView)
    {
        _stopwatch       = new Stopwatch();
        _randomArrayItem = randomArrayItem.ThrowIfNull();
        _randomInt32     = randomInt32.ThrowIfNull();

        _time = Observable.Interval(TimeSpan.FromMilliseconds(5), RxApp.MainThreadScheduler)
            .Select(__ => new TimeSpan(_stopwatch.ElapsedTicks))
            .StartWith(TimeSpan.Zero)
            .ToProperty(this, x => x.Time, out _time);

        CheckCommand = CreateCommand(
            () =>
            {
                var correctResult = GetCorrectResult();

                if (correctResult != Result)
                {
                    throw new Exception("Uncorrect result.");
                }

                Count++;
                UpdateSample();
                Result = 0;
            });

        StartCommand = CreateCommand(
            () =>
            {
                CurrentTab = DailyGymnasticsTab.Calculator;
                _stopwatch.Start();
            });

        StopCommand = CreateCommand(
            () =>
            {
                _stopwatch.Stop();
                Statistic  = new DailyGymnasticsStatistic(Count, Count / (double)Time.Seconds);
                CurrentTab = DailyGymnasticsTab.Statistic;
                _stopwatch.Restart();
            });

        UpdateSample();
    }

    private void SetRandomOperation()
    {
        _operation = _randomArrayItem.GetRandom(_opetators);
    }

    private void SetRandomOperators()
    {
        _firstOperator  = _randomInt32.GetRandom();
        _secondOperator = _randomInt32.GetRandom();
    }

    private void UpdateSample()
    {
        SetRandomOperation();
        SetRandomOperators();
        Sample = $"{_firstOperator} {_operation} {_secondOperator} =";
    }

    private int GetCorrectResult()
    {
        switch (_operation)
        {
            case "*": return _firstOperator * _secondOperator;
            case "/": return _firstOperator / _secondOperator;
            case "+": return _firstOperator + _secondOperator;
            case "-": return _firstOperator - _secondOperator;
            default:  return 0;
        }
    }
}