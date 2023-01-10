namespace WorkTool.Core.Modules.SmsClub.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister dependencyInjectorRegister)
    {
        dependencyInjectorRegister.RegisterTransient(() => SmsSenderEndpointsOptions.Default);
        dependencyInjectorRegister.RegisterTransient(() => SmsSenderOptions.Default);

        dependencyInjectorRegister.RegisterTransient<
            ISmsClubSender<object>,
            SmsClubSender<object>
        >();

        dependencyInjectorRegister.RegisterReserveTransient<SmsClubSender<object>, HttpClient>(
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
