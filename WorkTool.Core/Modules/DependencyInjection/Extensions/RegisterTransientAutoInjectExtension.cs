namespace WorkTool.Core.Modules.DependencyInjection.Extensions;

public static class RegisterTransientAutoInjectExtension
{
    public static void RegisterTransientAutoInject<T, TParameter>(
        this IRegisterTransientAutoInject autoInject,
        Expression<Func<T, TParameter>> expression,
        Func<TParameter> del
    )
    {
        autoInject.RegisterTransientAutoInject(expression, (Delegate)del);
    }

    public static void RegisterTransientAutoInject<T, TParameter>(
        this IRegisterTransientAutoInject autoInject,
        Expression<Func<T, TParameter>> expression,
        Delegate del
    )
    {
        var member = expression.Body.ThrowIfIsNot<MemberExpression>().Member;
        autoInject.RegisterTransientAutoInject(
            new AutoInjectIdentifier(typeof(T), (AutoInjectMember)member),
            del
        );
    }

    public static void RegisterTransientAutoInject<T, TParameter>(
        this IRegisterTransientAutoInject autoInject,
        Expression<Func<T, TParameter>> expression
    )
    {
        autoInject.RegisterTransientAutoInject(expression, (TParameter t) => t);
    }
}
