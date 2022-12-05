namespace WorkTool.Core.Modules.AvaloniaUi.Views;

public class NullablePropertyInfoReactiveItemsView
    : ReactiveNullablePropertyInfoReactiveItemsControl<ViewModelBase>
{
    private readonly PropertyInfoTemplatedControlContext context;

    public NullablePropertyInfoReactiveItemsView(
        UiContext avaloniaUiContext,
        ViewModelBase viewModel,
        PropertyInfoTemplatedControlContext context
    )
    {
        this.context = context.ThrowIfNull();
        DataContext = viewModel.ThrowIfNull();
        avaloniaUiContext.InitView(this);
    }

    protected override void UpdateItems()
    {
        base.UpdateItems();

        if (Value is null)
        {
            return;
        }

        var properties = Value
            .GetType()
            .GetProperties(
                BindingFlags.Instance
                    | BindingFlags.Public
                    | BindingFlags.GetProperty
                    | BindingFlags.SetProperty
            );

        foreach (var property in properties)
        {
            var view = context.GetView(property.PropertyType).Invoke(Value, property);

            this.GetObservable(ValueProperty)
                .Subscribe(x => view.Object = x)
                .DisposeWith(CompositeDisposable);

            ItemProperties.Add(view);
        }
    }
}
