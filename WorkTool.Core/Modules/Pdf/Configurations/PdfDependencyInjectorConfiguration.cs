namespace WorkTool.Core.Modules.Pdf.Configurations;

public class PdfDependencyInjectorConfiguration : IDependencyInjectorConfiguration
{
    public void Configure(IDependencyInjectorBuilder dependencyInjectorBuilder)
    {
        dependencyInjectorBuilder.RegisterTransient<IPdfReader, PdfReaderService>();
    }
}
