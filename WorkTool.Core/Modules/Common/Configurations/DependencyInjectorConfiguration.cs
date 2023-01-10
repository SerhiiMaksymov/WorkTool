namespace WorkTool.Core.Modules.Common.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister dependencyInjectorRegister)
    {
        dependencyInjectorRegister.RegisterTransient<IDelay, DelayService>();

        dependencyInjectorRegister.RegisterTransient<
            ITaskCompletionSourceEnumerator,
            TaskCompletionSourceEnumerator
        >();

        dependencyInjectorRegister.RegisterTransient<
            IHumanizing<Exception, object>,
            ExceptionHumanizing
        >();

        dependencyInjectorRegister.RegisterTransient<
            IHumanizing<Exception, string>,
            ToStringHumanizing<Exception>
        >();

        dependencyInjectorRegister.RegisterTransient<
            IIdentifierGenerator<string>,
            RandomIdentifierGenerator<string>
        >();

        dependencyInjectorRegister.RegisterReserveTransient<
            RandomIdentifierGenerator<string>,
            IRandom<string>,
            RandomGuid
        >();
    }
}
