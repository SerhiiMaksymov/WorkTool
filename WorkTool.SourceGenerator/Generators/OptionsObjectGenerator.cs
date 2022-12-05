namespace WorkTool.SourceGenerator.Generators;

[Generator]
public class OptionsObjectGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        //if (!Debugger.IsAttached)
        // Debugger.Launch();

        try
        {
            context.RegisterForSyntaxNotifications(() => new OptionsObjectReceiver());
        }
        catch (Exception exception)
        {
            Log.Send.SendAsync(exception.ToString(), context.CancellationToken);
        }
    }

    public void Execute(GeneratorExecutionContext context)
    {
        //if (!Debugger.IsAttached)
        //Debugger.Launch();

        try
        {
            var index = 0u;

            if (context.SyntaxReceiver is not OptionsObjectReceiver receiver)
            {
                return;
            }

            if (context.Compilation is not CSharpCompilation compilation)
            {
                return;
            }

            if (compilation.SyntaxTrees[0].Options is not CSharpParseOptions)
            {
                return;
            }

            foreach (var type in receiver.Items)
            {
                var semanticModel = compilation.GetSemanticModel(type.Type.SyntaxTree);
                var @namespace = compilation.Assembly.MetadataName.ToNamespace();
                var extension = semanticModel.GetObjectOptions(type, @namespace);
                var text = extension.ToString();
                var name = this.GetGeneratedName(extension.Type.Name, index++);
                context.AddSource(name, text);
                context.CancellationToken.ThrowIfCancellationRequested();
            }
        }
        catch (Exception exception)
        {
            Log.Send.SendAsync(exception.ToString(), context.CancellationToken);
        }
    }
}
