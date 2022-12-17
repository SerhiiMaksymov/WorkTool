namespace WorkTool.Core.Modules.FileSystem.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorBuilder dependencyInjectorBuilder)
    {
        dependencyInjectorBuilder.RegisterTransient<IFileSystemRootGetter, FileSystemRootGetter>();
        dependencyInjectorBuilder.RegisterTransientAutoInject(
            (DiskUsageView view) => view.ViewModel
        );
    }
}
