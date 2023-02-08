namespace WorkTool.Core.Modules.FileSystem.Modules;

public class FileSystemModule : ModularSystemModule
{
    private static readonly DependencyInjector MainDependencyInjector;

    private const string IdString = "4febf46e-6639-44ef-8ce2-d5b03f8a3aaf";

    public static readonly Guid IdValue = Guid.Parse(IdString);

    static FileSystemModule()
    {
        var register = new ReadOnlyDependencyInjectorRegister();
        register.RegisterConfiguration<FileSystemDependencyInjectorConfiguration>();
        MainDependencyInjector = register.Build();
    }

    public FileSystemModule() : base(IdValue, MainDependencyInjector) { }
}
