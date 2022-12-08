namespace WorkTool.SourceGenerator.Extensions;

public static class SemanticModelExtension
{
    public static StructParameters GetPartStruct(
        this SemanticModel semanticModel,
        PartObjectParameters parameters,
        NamespaceOptions @namespace
    )
    {
        var typeInfo = semanticModel.GetTypeInfo(parameters.Type);
        var properties = typeInfo.Type.ThrowIfNull().GetPartProperties(parameters);

        return new StructParameters(
            AccessModifier.Public,
            StructIdentifier.Partial,
            new TypeParameters(
                @namespace,
                parameters.ClassName,
                Enumerable.Empty<TypeParameters>()
            ),
            Enumerable.Empty<GenericParameters>(),
            Enumerable.Empty<FieldParameters>(),
            Enumerable.Empty<ConstructorParameters>(),
            properties,
            Enumerable.Empty<MethodParameters>(),
            Enumerable.Empty<OperatorParameters>()
        );
    }

    public static ClassParameters GetPartClass(
        this SemanticModel semanticModel,
        PartObjectParameters parameters,
        NamespaceOptions @namespace
    )
    {
        var typeInfo = semanticModel.GetTypeInfo(parameters.Type);
        var properties = typeInfo.Type.ThrowIfNull().GetPartProperties(parameters);

        return new ClassParameters(
            AccessModifier.Public,
            false,
            true,
            new TypeParameters(
                @namespace,
                parameters.ClassName,
                Enumerable.Empty<TypeParameters>()
            ),
            false,
            Enumerable.Empty<GenericParameters>(),
            Enumerable.Empty<FieldParameters>(),
            properties,
            Enumerable.Empty<ConstructorParameters>(),
            Enumerable.Empty<MethodParameters>(),
            Enumerable.Empty<TypeParameters>()
        );
    }

    public static ClassParameters GetFluentExtension(
        this SemanticModel semanticModel,
        FluentObjectParameters parameters,
        CodeAnalysisTypeInfo typeInfo,
        NamespaceOptions @namespace,
        CancellationToken cancellationToken
    )
    {
        Log.Send.SendAsync(@namespace.ToString(), cancellationToken);
        var type    = typeInfo.Type.ThrowIfNull();
        var methods = type.GetFluentExtensionMethods().ToArray();

        foreach (var method in methods)
        {
            Log.Send.SendAsync(method.Name, cancellationToken);
        }

        var name = parameters.GetExtensionName(type);

        return new ClassParameters(
            AccessModifier.Public,
            true,
            true,
            new TypeParameters(@namespace, name, Enumerable.Empty<TypeParameters>()),
            false,
            Enumerable.Empty<GenericParameters>(),
            Enumerable.Empty<FieldParameters>(),
            Enumerable.Empty<PropertyParameters>(),
            Enumerable.Empty<ConstructorParameters>(),
            methods,
            Enumerable.Empty<TypeParameters>()
        );
    }

    public static ClassParameters GetFluentExtension(
        this SemanticModel semanticModel,
        FluentObjectParameters parameters,
        NamespaceOptions @namespace,
        CancellationToken cancellationToken
    )
    {
        var type = semanticModel.GetTypeInfo(parameters.Type);

        return semanticModel.GetFluentExtension(parameters, type, @namespace, cancellationToken);
    }

    public static ClassParameters GetObjectOptions(
        this SemanticModel semanticModel,
        OptionsObjectParameters parameters,
        NamespaceOptions @namespace
    )
    {
        var typeInfo = semanticModel.GetTypeInfo(parameters.Type);
        var type     = typeInfo.Type.ThrowIfNull();
        var name =
            $"{type.Name}{(parameters.Postfix.IsNullOrWhiteSpace() ? "Options" : parameters.Postfix)}";
        var properties = type.GetObjectOptionsParameters();

        return new ClassParameters(
            AccessModifier.Public,
            false,
            true,
            new TypeParameters(@namespace, name, Enumerable.Empty<TypeParameters>()),
            false,
            Enumerable.Empty<GenericParameters>(),
            Enumerable.Empty<FieldParameters>(),
            properties,
            Enumerable.Empty<ConstructorParameters>(),
            Enumerable.Empty<MethodParameters>(),
            Enumerable.Empty<TypeParameters>()
        );
    }

    public static StructParameters GetModelObject(
        this SemanticModel semanticModel,
        ModelObjectParameters parameters,
        NamespaceOptions @namespace
    )
    {
        var typeInfo               = semanticModel.GetTypeInfo(parameters.Type);
        var type                   = typeInfo.Type.ThrowIfNull();
        var name                   = type.Name.OptionsTypeNameToModelTypeName();
        var properties             = type.GetObjectOptionsParameters().ToArray();
        var constructors           = type.GetObjectOptionsConstructors(properties);
        var typeParameters         = type.ToTypeParameters(name);
        var genericsTypeParameters = type.GetGenericsParameters();
        var methods                = type.GetObjectOptionsMethods(properties);

        return new StructParameters(
            AccessModifier.Public,
            StructIdentifier.Partial,
            typeParameters,
            genericsTypeParameters,
            Enumerable.Empty<FieldParameters>(),
            constructors,
            properties,
            methods,
            Enumerable.Empty<OperatorParameters>()
        );
    }
}
