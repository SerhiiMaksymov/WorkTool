namespace WorkTool.Core.Modules.DependencyInjection.Extensions;

public static class RegisterScopeExtension
{
    public static void RegisterScope<T, TImp>(this IRegisterScope register)
    {
        register.RegisterScope(typeof(T), typeof(TImp));
    }

    public static void RegisterScope(this IRegisterScope registerTransient, Type id, Type impType)
    {
        var constructor = impType.GetSingleConstructor();

        if (constructor is null)
        {
            if (impType.IsValueType)
            {
                var lambdaNew = impType.ToNew();
                registerTransient.RegisterScope(id, lambdaNew);

                return;
            }

            throw new NotHaveConstructorException(impType);
        }

        var parameters = constructor.GetParameters();

        if (parameters.Length == 0)
        {
            var lambdaNew = impType.ToNew();
            registerTransient.RegisterScope(id, lambdaNew);

            return;
        }

        var expressions = parameters.Select(x => x.ParameterType.ToParameterAutoName()).ToArray();

        var expressionNew = constructor.ToNew(expressions);
        registerTransient.RegisterScope(id, expressionNew);
    }
}
