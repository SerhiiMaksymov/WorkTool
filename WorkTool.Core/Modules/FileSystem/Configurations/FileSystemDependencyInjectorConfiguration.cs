namespace WorkTool.Core.Modules.FileSystem.Configurations;

public readonly struct FileSystemDependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorRegister register)
    {
        register.RegisterTransient<IFileSystemRootGetter, FileSystemRootGetter>();
        register.RegisterTransient<IDirectoryService, DirectoryService>();
        register.RegisterTransient<FileSystemUiConfiguration>();
    }
}
