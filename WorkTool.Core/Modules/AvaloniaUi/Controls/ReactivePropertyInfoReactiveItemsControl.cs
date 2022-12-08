namespace WorkTool.Core.Modules.AvaloniaUi.Controls;

public class ReactivePropertyInfoReactiveItemsControl<TViewModel>
    : PropertyInfoReactiveItemsControl,
        IViewFor<TViewModel> where TViewModel : class
{
    public static readonly StyledProperty<TViewModel?> ViewModelProperty =
        AvaloniaProperty.Register<
            ReactivePropertyInfoReactiveItemsControl<TViewModel>,
            TViewModel?
        >(nameof(ViewModel));

    public ReactivePropertyInfoReactiveItemsControl()
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
        set
        {
            switch (value)
            {
                case null:
                    ViewModel = null;

                    return;
                case TViewModel viewModel:
                    ViewModel = viewModel;

                    return;
                default:
                    throw new TypeInvalidCastException(typeof(TViewModel), value.GetType());
            }
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        ViewModel = DataContext as TViewModel;
    }

    private void OnViewModelChanged(object? value)
    {
        if (value is null)
        {
            ClearValue(DataContextProperty);
        }
        else if (DataContext != value)
        {
            DataContext = value;
        }
    }
}
