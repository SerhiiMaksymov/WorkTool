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

    Target Refactoring =>
        _ =>
            _.Executes(async () =>
            {
                var output = new StringBuilder();
                output.Append(Environment.NewLine);
                var errorOutput = new StringBuilder();
                errorOutput.Append(Environment.NewLine);
                var errorMassage = $"{nameof(Solution)}.{nameof(Solution.Directory)}";

                var directory =
                    Solution.Directory ?? throw new NullReferenceException(errorMassage);

                await Cli.Wrap("dotnet")
                    .WithWorkingDirectory(directory)
                    .WithArguments("csharpier ./")
                    .WithStandardOutputPipe(PipeTarget.ToStringBuilder(output))
                    .WithStandardErrorPipe(PipeTarget.ToStringBuilder(errorOutput))
                    .ExecuteAsync();

                var errorOutputString = errorOutput.ToString();
                var outputString = output.ToString();

                if (!string.IsNullOrWhiteSpace(errorOutputString))
                {
                    Log.Error(errorOutputString);
                }

                if (!string.IsNullOrWhiteSpace(outputString))
                {
                    Log.Information(outputString);
                }
            });

    Target Clean =>
        _ =>
            _.DependsOn(Restore)
                .Executes(() =>
                {
                    DotNetClean(_ => _.SetConfiguration(Configuration));
                });

    Target Restore =>
        _ =>
            _.DependsOn(Refactoring)
                .Executes(() =>
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
