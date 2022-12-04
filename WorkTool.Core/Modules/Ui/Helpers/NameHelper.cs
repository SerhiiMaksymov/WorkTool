namespace WorkTool.Core.Modules.Ui.Helpers;

public static class NameHelper
{
    public static string GetNameFunction(
    Type                             serviceType,
    IEnumerable<ParameterExpression> parameters,
    MethodInfo                       method)
    {
        var parametersList = parameters.Select(x => x.Type.Name).JoinString();

        return $"{GetTypeFullName(serviceType)}{parametersList}{method.Name}";
    }

    public static string GetNameFunction(
    Type                    serviceType,
    IEnumerable<Expression> parameters,
    MethodInfo              method)
    {
        var parametersList = parameters.Select(x => x.Type.Name).JoinString();

        return $"{GetTypeFullName(serviceType)}{parametersList}{method.Name}";
    }

    public static string GetNameFunction(Expression expression)
    {
        var lambda               = expression.As<LambdaExpression>();
        var methodCallExpression = lambda.Body.As<MethodCallExpression>();
        var genericArguments     = expression.Type.GetGenericArguments();

        return GetNameFunction(genericArguments.First(), methodCallExpression.Arguments, methodCallExpression.Method);
    }

    public static string GetNameAddTabItemFunction(Type tabControlViewType, Type contentType)
    {
        return $"AddTabItem{GetTypeFullName(contentType)}For{GetTypeFullName(tabControlViewType)}";
    }

    public static string GetNameAddTabItemFunction<TTabControlView, TContent>() where TTabControlView : ITabControlView
    {
        return GetNameAddTabItemFunction(typeof(TTabControlView), typeof(TContent));
    }

    public static string GetTypeFullName(Type type)
    {
        var genericArguments = type.GetGenericArguments();

        if (genericArguments.IsEmpty())
        {
            return type.Name;
        }

        return $"{type.Name}[{genericArguments.Select(x => GetTabItemHeader(x)).JoinString(", ")}]";
    }

    public static string GetTypeFullName<TType>()
    {
        return GetTypeFullName(typeof(TType));
    }

    public static string GetTabItemHeader(Type type)
    {
        return GetTypeFullName(type);
    }

    public static string GetTabItemHeader<TType>()
    {
        return GetTabItemHeader(typeof(TType));
    }
}