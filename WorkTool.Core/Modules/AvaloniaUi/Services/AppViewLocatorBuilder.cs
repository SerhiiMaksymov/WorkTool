namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class AppViewLocatorBuilder : IBuilder<AppViewLocator>
{
    private readonly IResolver resolver;
    private readonly Dictionary<Type, Type> resolveViewDictionary;

    public AppViewLocatorBuilder(IResolver resolver)
    {
        this.resolver = resolver;
        resolveViewDictionary = new Dictionary<Type, Type>();
    }

    public void AddResolve(Type viewModelType, Type viewType)
    {
        resolveViewDictionary.Add(viewModelType, viewType);
    }

    public AppViewLocator Build()
    {
        return new AppViewLocator(resolveViewDictionary, resolver);
    }
}
