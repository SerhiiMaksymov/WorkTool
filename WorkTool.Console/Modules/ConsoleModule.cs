using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;

using Material.Styles.Themes;

using Microsoft.EntityFrameworkCore;

using WorkTool.Console.Configurations;
using WorkTool.Console.Interfaces;
using WorkTool.Console.Services;
using WorkTool.Core.Modules.AvaloniaUi.Controls;
using WorkTool.Core.Modules.AvaloniaUi.Helpers;
using WorkTool.Core.Modules.AvaloniaUi.Views;
using WorkTool.Core.Modules.MaterialDesign.Services;

namespace WorkTool.Console.Modules;

public class ConsoleModule : ModularSystemModule
{
    private static readonly DependencyInjector MainDependencyInjector;

    private const string IdString = "e03a6f47-ba8c-45bc-bf8e-27837a41a740";

    public static readonly Guid IdValue = Guid.Parse(IdString);

    static ConsoleModule()
    {
        var register = new ReadOnlyDependencyInjectorRegister();
        register.RegisterTransient<MainView>();
        register.RegisterTransient<ConsoleUiConfiguration>();
        register.RegisterTransient<IUnitOfWork, UnitOfWork>();
        register.RegisterTransient<DbContext, ConsoleSqliteDbContext>();

        register.RegisterTransient<IControl>(
            (MainView content) => new DialogControl().SetContent(content)
        );

        register.RegisterTransient<IEnumerable<IApplicationCommandLine>>(
            (AvaloniaUiApplicationCommandLine avaloniaUiApplicationCommandLine) =>
                new IApplicationCommandLine[] { avaloniaUiApplicationCommandLine }
        );

        register.RegisterTransient<AvaloniaUiApplicationCommandLine>(
            (AvaloniaUiApplication avaloniaUiApplication) =>
                new AvaloniaUiApplicationCommandLine(avaloniaUiApplication)
        );

        register.RegisterTransient<IEnumerable<IStyle>>(
            (
                FluentTheme fluentTheme,
                MaterialTheme materialTheme,
                MaterialIconsTheme materialIconsTheme,
                Uri uri
            ) =>
                new IStyle[]
                {
                    fluentTheme,
                    new StyleInclude(uri) { Source = UriBase.DataGridThemeFluentUri },
                    new StyleInclude(uri) { Source = UriBase.ControlsStylesUri },
                    materialTheme,
                    materialIconsTheme
                }
        );

        MainDependencyInjector = register.Build();
    }

    public ConsoleModule() : base(IdValue, MainDependencyInjector) { }
}
