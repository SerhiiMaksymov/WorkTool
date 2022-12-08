namespace WorkTool.Core.Modules.AvaloniaUi.Views;

public class PropertyInfoReactiveItemsView<TType> : PropertyInfoReactiveItemsView
    where TType : notnull
{
    public PropertyInfoReactiveItemsView(
        UiContext avaloniaUiContext,
        ViewModelBase viewModel,
        PropertyInfoTemplatedControlContext context,
        IResolver resolver
    ) : base(avaloniaUiContext, viewModel, context)
    {
        Value = resolver.Resolve<TType>();
        Title = typeof(TType).ToString();
    }
}

public class PropertyInfoReactiveItemsView : ReactivePropertyInfoReactiveItemsControl<ViewModelBase>
{
    private const BindingFlags PropertiesFlags =
        BindingFlags.Instance
        | BindingFlags.Public
        | BindingFlags.GetProperty
        | BindingFlags.SetProperty;
    private readonly PropertyInfoTemplatedControlContext context;

    public PropertyInfoReactiveItemsView(
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
        ItemProperties.Clear();

        if (Value is null)
        {
            return;
        }

        var properties = Value.GetType().GetProperties(PropertiesFlags);

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
