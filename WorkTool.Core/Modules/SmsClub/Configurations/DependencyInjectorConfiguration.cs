namespace WorkTool.Core.Modules.SmsClub.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterTransient(() => SmsSenderEndpointsOptions.Default);
        register.RegisterTransient(() => SmsSenderOptions.Default);
        register.RegisterTransient<SmsClubUiConfiguration>();
        register.RegisterTransient<ISmsClubSender<object>, SmsClubSender<object>>();

        register.RegisterTransient<SmsClubSender<object>>(
            (
                IConfiguration configuration,
                SmsSenderEndpointsOptions endpointsOptions,
                SmsSenderOptions options,
                IDelay delay
            ) =>
            {
                var host =
                    configuration[SmsClubSender.ConfigHostPath]?.ToUri()
                    ?? SmsClubSender.DefaultHostUri;

                var apiKey = SmsClubSender.EnvironmentApiKeyName
                    .GetEnvironmentVariable()
                    .ThrowIfNull()
                    .ToAuthenticationBearer();

                var httpClient = new HttpClient().SetBaseAddress(host).SetAuthorization(apiKey);

                return new SmsClubSender<object>(httpClient, endpointsOptions, options, delay);
            }
        );
    }
}
