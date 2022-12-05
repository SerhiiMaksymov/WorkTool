namespace WorkTool.SourceGenerator.Extensions;

public static class TypeSymbolExtension
{
    public static IEnumerable<PropertyParameters> GetPartProperties(
        this ITypeSymbol type,
        PartObjectParameters parameters
    )
    {
        var propertySymbols = type.GetMembers().OfType<IPropertySymbol>();

        foreach (var propertySymbol in propertySymbols)
        {
            var partPropertyAttribute = propertySymbol
                .GetAttributes()
                .FirstOrDefault(
                    x =>
                        x.AttributeClass.MetadataName.Equals(nameof(PartPropertyAttribute))
                        && x.ConstructorArguments[0].Value.Equals(parameters.Name)
                );

            if (partPropertyAttribute is null)
            {
                continue;
            }

            yield return propertySymbol.ToPropertyParameters();
        }
    }

    public static bool IsPrimitiveType(this ITypeSymbol type)
    {
        return type.SpecialType.IsPrimitiveType();
    }

    public static TypeParameters ToTypeParameters(this ITypeSymbol type)
    {
        return type.ToTypeParameters(type.Name);
    }

    public static TypeParameters ToTypeParameters(this ITypeSymbol type, string name)
    {
        var namespaceOptions = type.ToNamespaceOptions();
        var genericsTypeParameters = type.GetGenericsTypeParameters();
        var result = new TypeParameters(namespaceOptions, name, genericsTypeParameters);

        return result;
    }

    public static NamespaceOptions ToNamespaceOptions(this ITypeSymbol type)
    {
        return type.ContainingSymbol.ToString().ToNamespace();
    }

    public static IEnumerable<MethodParameters> GetObjectOptionsMethods(
        this ITypeSymbol type,
        IEnumerable<PropertyParameters> properties
    )
    {
        var typeParameters = type.ToTypeParameters();

        yield return new MethodParameters(
            AccessModifier.Public,
            false,
            typeParameters,
            "ToOptions",
            Enumerable.Empty<GenericParameters>(),
            Enumerable.Empty<ArgumentParameters>(),
            $"return new {typeParameters}(){{{properties.Select(x => $"{x.Name} = {x.Name},").JoinString()}}};"
        );
    }

    public static IEnumerable<TypeParameters> GetGenericsTypeParameters(this ITypeSymbol type)
    {
        if (type is not INamedTypeSymbol namedTypeSymbol)
        {
            yield break;
        }

        for (var index = 0; index < namedTypeSymbol.TypeArguments.Length; index++)
        {
            var typeArgument = namedTypeSymbol.TypeArguments[index];
            var typeParameter = namedTypeSymbol.TypeParameters[index];

            if (typeArgument.Name.EndsWith(nameof(GenericMark)))
            {
                yield return new TypeParameters(
                    null,
                    typeParameter.Name,
                    Enumerable.Empty<TypeParameters>()
                );
            }
            else if (typeParameter.Name == typeArgument.Name)
            {
                yield return typeParameter.ToTypeParameters();
            }
            else
            {
                yield return typeArgument.ToTypeParameters();
            }
        }
    }

    public static IEnumerable<GenericParameters> GetGenericsParameters(this ITypeSymbol type)
    {
        if (type is not INamedTypeSymbol namedTypeSymbol)
        {
            yield break;
        }

        for (var index = 0; index < namedTypeSymbol.TypeArguments.Length; index++)
        {
            var typeArgument = namedTypeSymbol.TypeArguments[index];
            var typeParameter = namedTypeSymbol.TypeParameters[index];

            if (typeArgument.Name.EndsWith(nameof(GenericMark)))
            {
                yield return new GenericParameters(
                    typeParameter.Name,
                    false,
                    GenericOptionsType.None,
                    Enumerable.Empty<TypeParameters>()
                );
            }
            else
            {
                yield return new GenericParameters(
                    typeParameter.Name,
                    false,
                    GenericOptionsType.None,
                    new[] { typeArgument.ToTypeParameters() }
                );
            }
        }
    }

    public static bool IsEnumerableType(this ITypeSymbol type)
    {
        if (type.SpecialType.IsEnumerableType())
        {
            return true;
        }

        var @namespace = $"{type.ContainingNamespace}.{type.Name}";

        if (Consts.EnumerableNames.Contains(@namespace))
        {
            return true;
        }

        return false;
    }

    public static bool IsType(this ITypeSymbol type, TypeParameters parameters)
    {
        var typeOptions = type.ToTypeParameters();

        if (typeOptions.ToString() == parameters.ToString())
        {
            return true;
        }

        foreach (var @interface in type.Interfaces)
        {
            var collectionType = @interface.GetCollectionType();

            if (collectionType.ToString() == parameters.ToString())
            {
                return true;
            }
        }

        if (type.BaseType is not null)
        {
            return type.BaseType.IsType(parameters);
        }

        return false;
    }

    public static TypeParameters? GetCollectionType(this ITypeSymbol type)
    {
        if (type is IArrayTypeSymbol)
        {
            return null;
        }

        var @namespace = $"{type.ContainingSymbol}.{type.Name}";

        if (Consts.CollectionNames.Contains(@namespace))
        {
            return type.ToTypeParameters();
        }

        foreach (var @interface in type.Interfaces)
        {
            var collectionType = @interface.GetCollectionType();

            if (collectionType is not null)
            {
                return collectionType;
            }
        }

        if (type.BaseType is not null)
        {
            return type.BaseType.GetCollectionType();
        }

        return null;
    }

    public static IEnumerable<ISymbol> GetAllMembers(this ITypeSymbol type)
    {
        foreach (var member in type.GetMembers())
        {
            yield return member;
        }

        if (type.BaseType is null)
        {
            yield break;
        }

        foreach (var member in type.BaseType.GetAllMembers())
        {
            yield return member;
        }
    }

    public static IEnumerable<PropertyParameters> GetModelObjectProperties(this ITypeSymbol type)
    {
        var members = type.GetMembers();

        var getProperties = members
            .OfType<IPropertySymbol>()
            .Where(
                x =>
                    x.DeclaredAccessibility == Accessibility.Public
                    && x.GetMethod is not null
                    && x.GetMethod.DeclaredAccessibility == Accessibility.Public
                    && !x.IsStatic
            );

        foreach (var property in getProperties)
        {
            if (property.Name.StartsWith("this["))
            {
                continue;
            }

            yield return new PropertyParameters(
                property.Name,
                property.GetMethod.ReturnType.ToTypeParameters(),
                AccessModifier.Public,
                new PropertyGetterOptions(),
                null,
                null
            );
        }
    }

    public static IEnumerable<PropertyParameters> GetObjectOptionsParameters(this ITypeSymbol type)
    {
        var members = type.GetMembers();

        var getProperties = members
            .OfType<IPropertySymbol>()
            .Where(
                x =>
                    x.DeclaredAccessibility == Accessibility.Public
                    && x.GetMethod is not null
                    && x.GetMethod.DeclaredAccessibility == Accessibility.Public
                    && !x.IsStatic
            );

        foreach (var property in getProperties)
        {
            if (property.Name.StartsWith("this["))
            {
                continue;
            }

            if (property.GetMethod.ReturnType.Name.EndsWith(nameof(GenericMark)))
            {
                yield return new PropertyParameters(
                    property.Name,
                    new TypeParameters(
                        null,
                        property.GetMethod.OriginalDefinition.ReturnType.Name,
                        Enumerable.Empty<TypeParameters>()
                    ),
                    AccessModifier.Public,
                    new PropertyGetterOptions(),
                    null,
                    null
                );
            }
            else
            {
                yield return new PropertyParameters(
                    property.Name,
                    property.GetMethod.ReturnType.ToTypeParameters(),
                    AccessModifier.Public,
                    new PropertyGetterOptions(),
                    null,
                    null
                );
            }
        }
    }

    public static IEnumerable<ConstructorParameters> GetObjectOptionsConstructors(
        this ITypeSymbol type,
        IEnumerable<PropertyParameters> properties
    )
    {
        var typeParameters = type.ToTypeParameters(type.Name.OptionsTypeNameToModelTypeName());

        yield return new ConstructorParameters(
            AccessModifier.Public,
            typeParameters,
            properties.ToArguments(),
            properties
                .Select(x => $"{x.Name} = {x.Name.PropertyNameToArgumentName()};")
                .JoinString(Environment.NewLine),
            null
        );
    }

    public static IEnumerable<MethodParameters> GetFluentExtensionMethods(this ITypeSymbol type)
    {
        var outPutType = type.ToTypeParameters();
        var argumentName = type.Name.PropertyNameToArgumentName();
        var members = type.GetMembers();

        var setProperties = members
            .OfType<IPropertySymbol>()
            .Where(
                x =>
                    x.DeclaredAccessibility == Accessibility.Public
                    && x.SetMethod is not null
                    && x.SetMethod.DeclaredAccessibility == Accessibility.Public
                    && !x.IsStatic
            );

        var getProperties = members
            .OfType<IPropertySymbol>()
            .Where(
                x =>
                    x.DeclaredAccessibility == Accessibility.Public
                    && x.GetMethod is not null
                    && x.GetMethod.DeclaredAccessibility == Accessibility.Public
                    && !x.IsStatic
            );

        foreach (var property in setProperties)
        {
            var propertyArgumentName = property.Name.PropertyNameToArgumentName();
            var generic = $"T{outPutType.Name}";

            if (property.Name.StartsWith("this["))
            {
                continue;
            }

            yield return new MethodParameters(
                AccessModifier.Public,
                true,
                new TypeParameters(null, generic, Enumerable.Empty<TypeParameters>()),
                $"Set{property.Name}",
                new GenericParameters[]
                {
                    new(generic, false, GenericOptionsType.None, new[] { outPutType })
                },
                new ArgumentParameters[]
                {
                    new(
                        true,
                        new TypeParameters(null, generic, Enumerable.Empty<TypeParameters>()),
                        argumentName
                    ),
                    new(false, property.Type.ToTypeParameters(), propertyArgumentName)
                },
                $@"{
                    argumentName
                }.{
                    property.Name
                } = {
                    propertyArgumentName
                };
return {
    argumentName
};"
            );

            if (property.Type.TypeKind == TypeKind.Enum)
            {
                var namedTypeSymbol = property.Type as INamedTypeSymbol;

                foreach (var memberName in namedTypeSymbol.MemberNames)
                {
                    if (Consts.EnumDefaultMembers.Contains(memberName))
                    {
                        continue;
                    }

                    yield return new MethodParameters(
                        AccessModifier.Public,
                        true,
                        new TypeParameters(null, generic, Enumerable.Empty<TypeParameters>()),
                        $"Set{property.Name}{memberName}",
                        new GenericParameters[]
                        {
                            new(generic, false, GenericOptionsType.None, new[] { outPutType })
                        },
                        new ArgumentParameters[]
                        {
                            new(
                                true,
                                new TypeParameters(
                                    null,
                                    generic,
                                    Enumerable.Empty<TypeParameters>()
                                ),
                                argumentName
                            )
                        },
                        $@"{
                            argumentName
                        }.{
                            property.Name
                        } = {
                            property.Type.ToTypeParameters()
                        }.{
                            memberName
                        };
return {
    argumentName
};"
                    );
                }
            }
        }

        foreach (var property in getProperties)
        {
            if (property.Name.StartsWith("this["))
            {
                continue;
            }

            var collectionType = property.GetMethod.ReturnType.GetCollectionType();

            if (collectionType is null)
            {
                continue;
            }

            var itemType = collectionType.Value.Generics.Single();
            var generic = $"T{outPutType.Name}";
            var itemArgumentName = "item";

            yield return new MethodParameters(
                AccessModifier.Public,
                true,
                new TypeParameters(null, generic, Enumerable.Empty<TypeParameters>()),
                $"Add{property.Name.PropertyNameToSingle()}",
                new GenericParameters[]
                {
                    new(generic, false, GenericOptionsType.None, new[] { outPutType })
                },
                new ArgumentParameters[]
                {
                    new(
                        true,
                        new TypeParameters(null, generic, Enumerable.Empty<TypeParameters>()),
                        argumentName
                    ),
                    new(false, itemType, itemArgumentName)
                },
                $@"{
                    argumentName
                }.{
                    property.Name
                }.Add({
                    itemArgumentName
                });
return {
    argumentName
};"
            );

            yield return new MethodParameters(
                AccessModifier.Public,
                true,
                new TypeParameters(null, generic, Enumerable.Empty<TypeParameters>()),
                $"Add{property.Name}",
                new GenericParameters[]
                {
                    new(generic, false, GenericOptionsType.None, new[] { outPutType })
                },
                new ArgumentParameters[]
                {
                    new(
                        true,
                        new TypeParameters(null, generic, Enumerable.Empty<TypeParameters>()),
                        argumentName
                    ),
                    new(
                        false,
                        typeof(IEnumerable<object>).ToTypeOptions(
                            new[] { itemType },
                            new[] { typeof(object).ToTypeOptions() }
                        ),
                        $"{itemArgumentName}s"
                    )
                },
                $@"foreach(var {
                    itemArgumentName
                } in {
                    itemArgumentName
                }s)
    {
        argumentName
    }.{
        property.Name
    }.Add({
        itemArgumentName
    });
return {
    argumentName
};"
            );
        }
    }
}
