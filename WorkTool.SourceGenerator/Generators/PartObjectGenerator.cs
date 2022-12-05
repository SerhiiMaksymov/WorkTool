namespace WorkTool.SourceGenerator.Generators;

[Generator]
public class PartObjectGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        //if (!Debugger.IsAttached)
        //Debugger.Launch();

        try
        {
            context.RegisterForSyntaxNotifications(() => new PartObjectReceiver());
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

            if (context.SyntaxReceiver is not PartObjectReceiver receiver)
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

                switch (type.ObjectType)
                {
                    case ObjectType.Class:
                    {
                        var extension = semanticModel.GetPartClass(type, @namespace);
                        var text = extension.ToString();
                        var name = this.GetGeneratedName(extension.Type.Name, index++);
                        context.AddSource(name, text);
                        context.CancellationToken.ThrowIfCancellationRequested();

                        break;
                    }
                    case ObjectType.Struct:
                    {
                        var extension = semanticModel.GetPartStruct(type, @namespace);
                        var text = extension.ToString();
                        var name = this.GetGeneratedName(extension.Type.Name, index++);
                        context.AddSource(name, text);
                        context.CancellationToken.ThrowIfCancellationRequested();

                        break;
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Log.Send.SendAsync(exception.ToString(), context.CancellationToken);
        }
    }
}
