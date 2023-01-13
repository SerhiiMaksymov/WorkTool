namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class AppViewLocator : IViewLocator
{
    private readonly Dictionary<Type, Type> resolveViewDictionary;
    private readonly IResolver resolver;

    public AppViewLocator(IReadOnlyDictionary<Type, Type> resolveViewDictionary, IResolver resolver)
    {
        this.resolver = resolver;
        this.resolveViewDictionary = new Dictionary<Type, Type>(resolveViewDictionary);
    }

    public IViewFor ResolveView<T>(T? viewModel, string? contract = null)
    {
        if (resolveViewDictionary.TryGetValue(typeof(T), out var viewType))
        {
            return CreateViewFor(viewType, viewModel);
        }

        viewType = GetViewTypeOrNull(typeof(T));

        if (viewType is null)
        {
            throw new ArgumentOutOfRangeException(nameof(viewModel));
        }

        return CreateViewFor(viewType, viewModel);
    }

    private Type? GetViewTypeOrNull(Type viewModelType)
    {
        var assembly = viewModelType.Assembly;

        if (viewModelType.Namespace.IsNullOrWhiteSpace())
        {
            return null;
        }

        var ns = viewModelType.Namespace
            .Replace(".ViewModels.", ".Views.")
            .Replace(".ViewModels", ".Views");

        var viewTypeName = $"{ns}.{viewModelType.Name}"[..^5];
        var viewType = assembly.GetType(viewTypeName);

        return viewType;
    }

    private IViewFor CreateViewFor<TViewModel>(Type viewType, TViewModel viewModel)
    {
        var view = resolver.Resolve(viewType);

        if (view is IDataContextProvider dataContextProvider)
        {
            dataContextProvider.DataContext = viewModel;
        }

        return view.ThrowIfIsNot<IViewFor>();
    }
}
