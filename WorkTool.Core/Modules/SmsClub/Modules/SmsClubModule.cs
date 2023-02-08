namespace WorkTool.Core.Modules.SmsClub.Modules;

public class SmsClubModule : ModularSystemModule
{
    private static readonly DependencyInjector MainDependencyInjector;

    private const string IdString = "60d733ee-0aea-4d8d-bbf2-4eb3f0aedad9";

    public static readonly Guid IdValue = Guid.Parse(IdString);

    static SmsClubModule()
    {
        var register = new ReadOnlyDependencyInjectorRegister();
        register.RegisterConfiguration<SmsClubDependencyInjectorConfiguration>();
        MainDependencyInjector = register.Build();
    }

    public SmsClubModule() : base(IdValue, MainDependencyInjector) { }
}
