using System.Diagnostics;

namespace WorkTool.SourceGenerator.Generators;

[Generator]
public class FluentObjectGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        //if (!Debugger.IsAttached)
            //Debugger.Launch();

        try
        {
            context.RegisterForSyntaxNotifications(() => new FluentObjectReceiver());
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

            if (context.SyntaxReceiver is not FluentObjectReceiver receiver)
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

            foreach (var fluentObjectParameters in receiver.Items)
            {
                var semanticModel = compilation.GetSemanticModel(
                    fluentObjectParameters.Type.SyntaxTree
                );
                var typeInfo = semanticModel.GetTypeInfo(fluentObjectParameters.Type);
                Log.Send.SendAsync(
                    $"Fluent Object: {typeInfo.Type.ThrowIfNull().Name}",
                    context.CancellationToken
                );
                var @namespace = compilation.Assembly.MetadataName.ToNamespace();
                var extension = semanticModel.GetFluentExtension(
                    fluentObjectParameters,
                    typeInfo,
                    @namespace,
                    context.CancellationToken
                );
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
