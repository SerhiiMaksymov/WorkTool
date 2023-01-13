namespace WorkTool.Core.Modules.Common.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterTransient<IDelay, DelayService>();
        register.RegisterTransient<IHumanizing<Exception, object>, ExceptionHumanizing>();
        register.RegisterTransient<IHumanizing<Exception, string>, ToStringHumanizing<Exception>>();

        register.RegisterTransient<
            ITaskCompletionSourceEnumerator,
            TaskCompletionSourceEnumerator
        >();

        register.RegisterTransient<
            IIdentifierGenerator<string>,
            RandomIdentifierGenerator<string>
        >();

        register.RegisterReserveTransient<
            RandomIdentifierGenerator<string>,
            IRandom<string>,
            RandomGuid
        >();
    }
}
