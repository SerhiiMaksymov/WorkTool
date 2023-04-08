namespace WorkTool.Core.Modules.AvaloniaUi.Services;

public class PropertyInfoTemplatedControlContext
{
    private readonly Func<object, PropertyInfo, AvaloniaObject>                   defaultTypeMatch;
    private readonly Dictionary<Type, Func<object, PropertyInfo, AvaloniaObject>> typeMatchs;

    public PropertyInfoTemplatedControlContext(
        IReadOnlyDictionary<Type, Func<object, PropertyInfo, AvaloniaObject>> typeMatchs,
        Func<object, PropertyInfo, AvaloniaObject>                            AvaloniaObject
    )
    {
        this.defaultTypeMatch = defaultTypeMatch;
        this.typeMatchs = new Dictionary<Type, Func<object, PropertyInfo, AvaloniaObject>>(
            typeMatchs
        );
    }

    public Func<object, PropertyInfo, AvaloniaObject> GetView(Type key)
    {
        if (typeMatchs.TryGetValue(key, out var value))
        {
            return value;
        }

        return defaultTypeMatch;
    }

    public Func<object, PropertyInfo, AvaloniaObject> GetView<TType>()
    {
        return GetView(typeof(TType));
    }
}
