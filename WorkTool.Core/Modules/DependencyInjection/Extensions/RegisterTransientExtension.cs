namespace WorkTool.Core.Modules.DependencyInjection.Extensions;

public static class RegisterTransientExtension
{
    public static void RegisterTransientDel<T>(
        this IRegisterTransient registerTransient,
        Delegate del
    )
    {
        registerTransient.RegisterTransient(
            typeof(T),
            del.ToCall(del.Method.GetParameters().Select(x => x.ParameterType.ToVariableAutoName()))
        );
    }

    public static void RegisterTransient<T>(
        this IRegisterTransient registerTransient,
        Expression @delegate
    )
    {
        registerTransient.RegisterTransient(typeof(T), @delegate);
    }

    public static void RegisterTransient<T>(
        this IRegisterTransient registerTransient,
        Expression<Func<T>> func
    )
    {
        registerTransient.RegisterTransient(typeof(T), func);
    }

    public static void RegisterTransient<T>(this IRegisterTransient registerTransient)
        where T : notnull
    {
        registerTransient.RegisterTransient<T, T>();
    }

    public static void RegisterTransient(
        this IRegisterTransient registerTransient,
        Type id,
        Type impType
    )
    {
        var constructor = impType.GetSingleConstructor();

        if (constructor is null)
        {
            if (impType.IsValueType)
            {
                var lambdaNew = impType.ToNew();
                registerTransient.RegisterTransient(id, lambdaNew);

                return;
            }

            throw new NotHaveConstructorException(impType);
        }

        var parameters = constructor.GetParameters();

        if (parameters.Length == 0)
        {
            var lambdaNew = impType.ToNew();
            registerTransient.RegisterTransient(id, lambdaNew);

            return;
        }

        var expressions = parameters.Select(x => x.ParameterType.ToParameterAutoName()).ToArray();

        var expressionNew = constructor.ToNew(expressions);
        registerTransient.RegisterTransient(id, expressionNew);
    }

    public static void RegisterTransient<T, TImp>(this IRegisterTransient registerTransient)
        where TImp : notnull, T
    {
        registerTransient.RegisterTransient(typeof(T), typeof(TImp));
    }

    public static void RegisterTransient(this IRegisterTransient registerTransient, Type id)
    {
        registerTransient.RegisterTransient(id, id);
    }
}
