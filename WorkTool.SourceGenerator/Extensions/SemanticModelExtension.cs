namespace WorkTool.SourceGenerator.Extensions;

public static class SemanticModelExtension
{
    public static StructParameters GetPartStruct(this SemanticModel   semanticModel,
                                                 PartObjectParameters parameters,
                                                 NamespaceOptions     @namespace)
    {
        var typeInfo   = semanticModel.GetTypeInfo(parameters.Type);
        var properties = typeInfo.Type.GetPartProperties(parameters);

        return new StructParameters(
            AccessModifier.Public,
            StructIdentifier.Partial,
            new TypeParameters(@namespace, parameters.ClassName, Enumerable.Empty<TypeParameters>()),
            Enumerable.Empty<GenericParameters>(),
            Enumerable.Empty<FieldParameters>(),
            Enumerable.Empty<ConstructorParameters>(),
            properties,
            Enumerable.Empty<MethodParameters>(),
            Enumerable.Empty<OperatorParameters>());
    }

    public static ClassParameters GetPartClass(this SemanticModel   semanticModel,
                                               PartObjectParameters parameters,
                                               NamespaceOptions     @namespace)
    {
        var typeInfo   = semanticModel.GetTypeInfo(parameters.Type);
        var properties = typeInfo.Type.GetPartProperties(parameters);

        return new ClassParameters(
            AccessModifier.Public,
            false,
            true,
            new TypeParameters(@namespace, parameters.ClassName, Enumerable.Empty<TypeParameters>()),
            false,
            Enumerable.Empty<GenericParameters>(),
            Enumerable.Empty<FieldParameters>(),
            properties,
            Enumerable.Empty<ConstructorParameters>(),
            Enumerable.Empty<MethodParameters>(),
            Enumerable.Empty<TypeParameters>());
    }

    public static ClassParameters GetFluentExtension(this SemanticModel     semanticModel,
                                                     FluentObjectParameters parameters,
                                                     NamespaceOptions       @namespace)
    {
        var type    = semanticModel.GetTypeInfo(parameters.Type);
        var methods = type.Type.GetFluentExtensionMethods();
        var name    = parameters.GetExtensionName(type.Type);

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
            Enumerable.Empty<TypeParameters>());
    }

    public static ClassParameters GetObjectOptions(this SemanticModel      semanticModel,
                                                   OptionsObjectParameters parameters,
                                                   NamespaceOptions        @namespace)
    {
        var type = semanticModel.GetTypeInfo(parameters.Type);
        var name = $"{type.Type.Name}{(parameters.Postfix.IsNullOrWhiteSpace() ? "Options" : parameters.Postfix)}";
        var properties = type.Type.GetObjectOptionsParameters();

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
            Enumerable.Empty<TypeParameters>());
    }

    public static StructParameters GetModelObject(this SemanticModel    semanticModel,
                                                  ModelObjectParameters parameters,
                                                  NamespaceOptions      @namespace)
    {
        var type                   = semanticModel.GetTypeInfo(parameters.Type);
        var name                   = type.Type.Name.OptionsTypeNameToModelTypeName();
        var properties             = type.Type.GetObjectOptionsParameters();
        var constructors           = type.Type.GetObjectOptionsConstructors(properties);
        var typeParameters         = type.Type.ToTypeParameters(name);
        var genericsTypeParameters = type.Type.GetGenericsParameters();
        var methods                = type.Type.GetObjectOptionsMethods(properties);

        return new StructParameters(
            AccessModifier.Public,
            StructIdentifier.Partial,
            typeParameters,
            genericsTypeParameters,
            Enumerable.Empty<FieldParameters>(),
            constructors,
            properties,
            methods,
            Enumerable.Empty<OperatorParameters>());
    }
}