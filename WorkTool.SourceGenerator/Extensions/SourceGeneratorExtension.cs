namespace WorkTool.SourceGenerator.Extensions;

public static class SourceGeneratorExtension
{
    public static string GetGeneratedName<TSourceGenerator>(this TSourceGenerator generator, string name, uint index)
        where TSourceGenerator : ISourceGenerator
    {
        return $"{typeof(TSourceGenerator).Name}{name}{index}.generated.cs";
    }
}