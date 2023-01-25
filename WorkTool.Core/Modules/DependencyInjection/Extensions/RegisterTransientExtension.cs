namespace WorkTool.Core.Modules.DependencyInjection.Extensions;

public static class RegisterTransientExtension
{
    public static void RegisterTransient<T>(
        this IRegisterTransient registerTransient,
        Delegate @delegate
    )
    {
        registerTransient.RegisterTransient(typeof(T), @delegate);
    }

    public static void RegisterTransient<T>(this IRegisterTransient registerTransient, Func<T> func)
    {
        registerTransient.RegisterTransient(typeof(T), func);
    }

    public static void RegisterTransient<T>(this IRegisterTransient registerTransient)
        where T : notnull
    {
        registerTransient.RegisterTransient<T, T>();
    }

    public static void RegisterTransient<T, TImp>(this IRegisterTransient registerTransient)
        where TImp : notnull, T
    {
        var impType = typeof(TImp);
        var constructor = impType.GetSingleConstructor();

        if (constructor is null)
        {
            if (impType.IsValueType)
            {
                var func = impType.ToNew().Lambda().Compile();
                registerTransient.RegisterTransient<T>(func);

                return;
            }

            throw new NotHaveConstructorException(impType);
        }

        var parameters = constructor.GetParameters();

        if (parameters.Length == 0)
        {
            var func = impType.ToNew().Lambda().Compile();
            registerTransient.RegisterTransient<T>(func);

            return;
        }

        var expressions = parameters
            .Select(x => x.ParameterType.ToParameter($"var{Guid.NewGuid():N}"))
            .ToArray();

        var expressionNew = constructor.ToNew(expressions);
        var del = expressionNew.Lambda(expressions).Compile();
        registerTransient.RegisterTransient<T>(del);
    }
}
