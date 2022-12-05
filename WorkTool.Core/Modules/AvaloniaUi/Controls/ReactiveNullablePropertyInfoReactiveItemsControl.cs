namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class ReactiveNullablePropertyInfoReactiveItemsControl<TViewModel>
    : NullablePropertyInfoReactiveItemsControl,
        IViewFor<TViewModel> where TViewModel : class
{
    public static readonly StyledProperty<TViewModel> ViewModelProperty = AvaloniaProperty.Register<
        ReactiveNullablePropertyInfoReactiveItemsControl<TViewModel>,
        TViewModel
    >(nameof(ViewModel));

    public ReactiveNullablePropertyInfoReactiveItemsControl()
    {
        this.WhenActivated(_ => { });

        this.GetObservable(ViewModelProperty).Subscribe(OnViewModelChanged);
    }

    public TViewModel? ViewModel
    {
        get => GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TViewModel)value;
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        ViewModel = DataContext as TViewModel;
    }

    private void OnViewModelChanged(object value)
    {
        if (value == null)
        {
            ClearValue(DataContextProperty);
        }
        else if (DataContext != value)
        {
            DataContext = value;
        }
    }
}
