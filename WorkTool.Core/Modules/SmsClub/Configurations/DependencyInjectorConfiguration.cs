namespace WorkTool.Core.Modules.SmsClub.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorBuilder dependencyInjectorBuilder)
    {
        dependencyInjectorBuilder.RegisterTransient<ControlPanelView>(
            (ControlPanelViewModel viewModel) => new ControlPanelView { ViewModel = viewModel }
        );

        dependencyInjectorBuilder.RegisterTransient<
            ISmsClubSender<object>,
            SmsClubSender<object>
        >();

        dependencyInjectorBuilder.RegisterReserveTransient<SmsClubSender<object>, HttpClient>(
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
