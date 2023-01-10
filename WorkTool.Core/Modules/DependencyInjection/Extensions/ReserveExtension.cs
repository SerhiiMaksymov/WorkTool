namespace WorkTool.Core.Modules.DependencyInjection.Extensions;

public static class ReserveExtension
{
    public static void RegisterReserveTransient<T, TParameter>(
        this IRegisterReserveTransient registerReserveTransient,
        Delegate @delegate
    )
    {
        var type = typeof(T);

        foreach (var constructor in type.GetConstructors())
        {
            var parameters = constructor.GetParameters();

            foreach (var parameter in parameters)
            {
                if (parameter.ParameterType != typeof(TParameter))
                {
                    continue;
                }

                registerReserveTransient.RegisterReserveTransient(
                    new ReserveIdentifier(type, parameter),
                    @delegate
                );

                return;
            }
        }

        throw new NotFondConstructorException(typeof(T), typeof(TParameter));
    }

    public static void RegisterReserveTransient<T, TParameter, TValue>(
        this IRegisterReserveTransient registerReserveTransient
    )
    {
        registerReserveTransient.RegisterReserveTransient<T, TParameter>((TValue value) => value);
    }

    public static void RegisterReserveTransient<T, TParameter>(
        this IRegisterReserveTransient registerReserveTransient
    )
    {
        registerReserveTransient.RegisterReserveTransient<T, TParameter, TParameter>();
    }
}
