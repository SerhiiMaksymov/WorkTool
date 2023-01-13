namespace WorkTool.Core.Modules.SmsClub.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterTransient(() => SmsSenderEndpointsOptions.Default);
        register.RegisterTransient(() => SmsSenderOptions.Default);
        register.RegisterTransient<ISmsClubSender<object>, SmsClubSender<object>>();

        register.RegisterReserveTransient<SmsClubSender<object>, HttpClient>(
            (IConfiguration configuration) =>
            {
                var host =
                    configuration[SmsClubSender.ConfigHostPath]?.ToUri()
                    ?? SmsClubSender.DefaultHostUri;

                var apiKey = SmsClubSender.EnvironmentApiKeyName
                    .GetEnvironmentVariable()
                    .ThrowIfNull()
                    .ToAuthenticationBearer();

                var httpClient = new HttpClient().SetBaseAddress(host).SetAuthorization(apiKey);

                return httpClient;
            }
        );
    }
}
