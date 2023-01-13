namespace WorkTool.Core.Modules.Common.Models;

/*public readonly struct TypeInformation : IEquatable<TypeInformation>
{
    public TypeInformation(Type type)
    {
        Identifier            = new(type);
        Type                  = type;
        IsGenericType         = type.IsGenericType;
        GenericTypeArguments  = type.GenericTypeArguments.Select(x => (TypeInformation)x).ToArray();
        GenericTypeDefinition = new Ref<TypeInformation>(type.GetGenericTypeDefinition());
        Constructors          = type.GetConstructors();
        IsValueType           = type.IsValueType;
        Members               = type.GetMembers();
    }
    
    public TypeIdentifier          Identifier            { get; }
    public Type                    Type                  { get; }
    public bool                    IsGenericType         { get; }
    public Memory<TypeInformation> GenericTypeArguments  { get; }
    public Ref<TypeInformation>    GenericTypeDefinition { get; }
    public Memory<ConstructorInfo> Constructors          { get; }
    public bool                    IsValueType           { get; }
    public Memory<MemberInfo>      Members            { get; }

    public bool Equals(TypeInformation other)
    {
        return Identifier.Equals(other.Identifier);
    }

    public override bool Equals(object? obj)
    {
        return obj is TypeInformation other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Identifier.GetHashCode();
    }

    public static implicit operator TypeInformation(Type type)
    {
        return new(type);
    }
    
    public static bool operator ==(TypeInformation x, TypeInformation y)
    {
        return x.Equals(y);
    }
    
    public static bool operator !=(TypeInformation x, TypeInformation y)
    {
        return !x.Equals(y);
    }
}*/