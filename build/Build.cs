class Build : NukeBuild
{
    [Solution]
    public Solution Solution;

    [GitVersion]
    public GitVersion GitVersion;

    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild
        ? Configuration.Debug
        : Configuration.Release;

    Target Clean =>
        _ =>
            _.DependsOn(Restore)
                .Executes(() =>
                {
                    DotNetClean(_ => _.SetConfiguration(Configuration));
                });

    Target Restore =>
        _ =>
            _.Executes(() =>
            {
                DotNetRestore();
            });

    Target Test =>
        _ =>
            _.DependsOn(Compile)
                .Executes(() =>
                {
                    DotNetTest(
                        _ =>
                            _.EnableNoRestore()
                                .EnableNoBuild()
                                .EnableNoRestore()
                                .SetConfiguration(Configuration)
                    );
                });

    Target Compile =>
        _ =>
            _.DependsOn(Clean)
                .Executes(() =>
                {
                    DotNetBuild(_ => _.EnableNoRestore().SetConfiguration(Configuration));
                });
}
