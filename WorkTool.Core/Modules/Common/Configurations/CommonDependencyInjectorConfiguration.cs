namespace WorkTool.Core.Modules.Common.Configurations;

public readonly struct CommonDependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterTransient<IDelay, DelayService>();
        register.RegisterTransient<IHumanizing<Exception, object>, ExceptionHumanizing>();
        register.RegisterTransient<IHumanizing<Exception, string>, ToStringHumanizing<Exception>>();
        register.RegisterTransient<IRandom<Guid>, RandomGuid>();

        register.RegisterTransient<IRandom<string>>(
            (IRandom<Guid> randomGuid) => new RandomStringGuid(randomGuid, GuidFormats.Digits)
        );

        register.RegisterTransient<
            ITaskCompletionSourceEnumerator,
            TaskCompletionSourceEnumerator
        >();

        register.RegisterTransient<
            IIdentifierGenerator<string>,
            RandomIdentifierGenerator<string>
        >();
    }
}
