namespace WorkTool.Core.Modules.DependencyInjection.Models;

public readonly record struct AutoInjectMember
{
    private readonly string name;

    public AutoInjectMember(MemberInfo member)
    {
        switch (member)
        {
            case PropertyInfo property:
            {
                if (!property.CanWrite)
                {
                    throw new ArgumentException();
                }

                break;
            }
            case FieldInfo field:
            {
                if (field.IsInitOnly)
                {
                    throw new ArgumentException();
                }

                break;
            }
            default:
            {
                throw new TypeInvalidCastException(
                    new[] { typeof(PropertyInfo), typeof(FieldInfo) },
                    member.GetType()
                );
            }
        }

        name = member.Name;
    }

    public static implicit operator AutoInjectMember(MemberInfo member)
    {
        return new(member);
    }
}
