namespace WorkTool.Core.Modules.FileSystem.Configurations;

public readonly struct DependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister dependencyInjectorRegister)
    {
        dependencyInjectorRegister.RegisterTransient<IFileSystemRootGetter, FileSystemRootGetter>();
        dependencyInjectorRegister.RegisterTransient<IDirectoryService, DirectoryService>();
    }
}
