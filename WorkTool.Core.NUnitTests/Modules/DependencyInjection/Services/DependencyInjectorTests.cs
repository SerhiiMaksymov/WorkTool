namespace WorkTool.Core.NUnitTests.Modules.DependencyInjection.Services;

public class DependencyInjectorTests
{
    private static int structPropertyValue;
    private static int reserveStructPropertyValue;

    private ReadOnlyReadOnlyDependencyInjector? dependencyInjector;

    [SetUp]
    public void SetUp()
    {
        structPropertyValue = 15;
        reserveStructPropertyValue = 50;
    }

    [Test]
    public void Resolve_AutoProperty_SetProperty()
    {
        var type = typeof(ClassWithPropertySetter);
        var member = type.GetMember(nameof(ClassWithPropertySetter.Property))[0];

        dependencyInjector = CreateDependencyInjector(
            new Dictionary<AutoInjectIdentifier, InjectorItem>
            {
                {
                    new AutoInjectIdentifier(type, member),
                    new InjectorItem(InjectorItemType.Singleton, () => structPropertyValue)
                }
            }
        );

        var value = dependencyInjector.Resolve(typeof(ClassWithPropertySetter));

        value.ThrowIfIsNot<ClassWithPropertySetter>().Property.Should().Be(structPropertyValue);
    }

    [TestCase(InjectorItemType.Transient, InjectorItemType.Transient)]
    [TestCase(InjectorItemType.Singleton, InjectorItemType.Singleton)]
    [TestCase(InjectorItemType.Singleton, InjectorItemType.Transient)]
    [TestCase(InjectorItemType.Transient, InjectorItemType.Singleton)]
    public void Resolve_ReserveParameter_ParameterSet(
        InjectorItemType injectorType,
        InjectorItemType reserveType
    )
    {
        var type = typeof(StructWith1Constructor);
        var parameter = type.GetConstructors()[0].GetParameters()[0];

        dependencyInjector = CreateDependencyInjector(
            new Dictionary<Type, InjectorItem>
            {
                { typeof(int), new InjectorItem(injectorType, () => structPropertyValue) }
            },
            new Dictionary<ReserveIdentifier, InjectorItem>
            {
                {
                    new ReserveIdentifier(type, parameter),
                    new InjectorItem(reserveType, () => reserveStructPropertyValue)
                }
            }
        );

        var value = dependencyInjector.Resolve(typeof(StructWith1Constructor));

        value
            .ThrowIfIsNot<StructWith1Constructor>()
            .Property.Should()
            .Be(reserveStructPropertyValue);
    }

    [Test]
    public void Resolve_UnCorrectInjectorItemType_Exception()
    {
        dependencyInjector = CreateDependencyInjector(
            new Dictionary<Type, InjectorItem>
            {
                { typeof(object), new InjectorItem((InjectorItemType)255, () => new object()) }
            }
        );

        var func = () => dependencyInjector.Resolve(typeof(object));

        func.Should().Throw<UnreachableException>();
    }

    [Test]
    public void Resolve_StructWithoutConstructor_StructInstance()
    {
        dependencyInjector = CreateDependencyInjector();

        var instance = dependencyInjector.Resolve(typeof(int));

        var int32 = instance.ThrowIfIsNot<int>();
        int32.Should().Be(0);
    }

    [Test]
    public void Resolve_StructWith1Constructor_StructInstance()
    {
        dependencyInjector = CreateDependencyInjector(
            new Dictionary<Type, InjectorItem>
            {
                {
                    typeof(int),
                    new InjectorItem(InjectorItemType.Singleton, () => structPropertyValue)
                }
            }
        );

        var instance = dependencyInjector.Resolve(typeof(StructWith1Constructor));

        var value = instance.ThrowIfIsNot<StructWith1Constructor>();
        value.Property.Should().Be(structPropertyValue);
    }

    [Test]
    public void Resolve_StructWith1ConstructorAndParameter_StructInstance()
    {
        dependencyInjector = CreateDependencyInjector(
            new Dictionary<Type, InjectorItem>
            {
                {
                    typeof(StructStructWith1ConstructorAndParameter),
                    new InjectorItem(
                        InjectorItemType.Transient,
                        () => new StructStructWith1ConstructorAndParameter(structPropertyValue)
                    )
                }
            }
        );

        var instance = dependencyInjector.Resolve(typeof(StructStructWith1ConstructorAndParameter));

        var value = instance.ThrowIfIsNot<StructStructWith1ConstructorAndParameter>();
        value.Property.Should().Be(structPropertyValue);
    }

    [Test]
    public void Resolve_SingletonLifeCircle_SingleObject()
    {
        dependencyInjector = CreateDependencyInjector(
            new Dictionary<Type, InjectorItem>
            {
                { typeof(object), new InjectorItem(InjectorItemType.Singleton, () => new object()) }
            }
        );

        var instance1 = dependencyInjector.Resolve(typeof(object));
        var instance2 = dependencyInjector.Resolve(typeof(object));

        instance1.Should().Be(instance2);
    }

    [Test]
    public void Resolve_TransientLifeCircle_NewObject()
    {
        dependencyInjector = CreateDependencyInjector(
            new Dictionary<Type, InjectorItem>
            {
                {
                    typeof(object),
                    new InjectorItem(InjectorItemType.Transient, () => new object())
                },
            }
        );

        var instance1 = dependencyInjector.Resolve(typeof(object));
        var instance2 = dependencyInjector.Resolve(typeof(object));

        instance1.Should().NotBe(instance2);
    }

    [Test]
    public void Resolve_UseFunc_Instance()
    {
        dependencyInjector = CreateDependencyInjector(
            new Dictionary<Type, InjectorItem>
            {
                {
                    typeof(StructStructWith1ConstructorAndParameter),
                    new InjectorItem(
                        InjectorItemType.Transient,
                        (int value) => new StructStructWith1ConstructorAndParameter(value)
                    )
                },
                {
                    typeof(int),
                    new InjectorItem(InjectorItemType.Transient, () => structPropertyValue)
                }
            }
        );

        var instance = dependencyInjector.Resolve(typeof(StructStructWith1ConstructorAndParameter));

        var value = instance.ThrowIfIsNot<StructStructWith1ConstructorAndParameter>();
        value.Property.Should().Be(structPropertyValue);
    }

    [Test]
    public void Resolve_ClassWithoutConstructors_Exception()
    {
        dependencyInjector = CreateDependencyInjector();

        var func = () => dependencyInjector.Resolve(typeof(ClassWithoutConstructors));

        func.Should()
            .Throw<NotHaveConstructorException>()
            .And.Type.Should()
            .Be(typeof(ClassWithoutConstructors));
    }

    [Test]
    public void Resolve_Class2Constructors_Exception()
    {
        dependencyInjector = CreateDependencyInjector();

        var func = () => dependencyInjector.Resolve(typeof(ClassWith2Constructors));

        func.Should()
            .Throw<ToManyConstructorsException>()
            .And.Type.Should()
            .Be(typeof(ClassWith2Constructors));
    }

    private ReadOnlyReadOnlyDependencyInjector CreateDependencyInjector()
    {
        return CreateDependencyInjector(
            ReadOnlyDictionary<Type, InjectorItem>.Empty,
            ReadOnlyDictionary<ReserveIdentifier, InjectorItem>.Empty,
            ReadOnlyDictionary<AutoInjectIdentifier, InjectorItem>.Empty,
            RandomStringGuid.Digits
        );
    }

    private ReadOnlyReadOnlyDependencyInjector CreateDependencyInjector(
        IReadOnlyDictionary<AutoInjectIdentifier, InjectorItem> autoInjects
    )
    {
        return CreateDependencyInjector(
            ReadOnlyDictionary<Type, InjectorItem>.Empty,
            ReadOnlyDictionary<ReserveIdentifier, InjectorItem>.Empty,
            autoInjects,
            RandomStringGuid.Digits
        );
    }

    private ReadOnlyReadOnlyDependencyInjector CreateDependencyInjector(
        IReadOnlyDictionary<Type, InjectorItem> injectors
    )
    {
        return CreateDependencyInjector(
            injectors,
            ReadOnlyDictionary<ReserveIdentifier, InjectorItem>.Empty,
            ReadOnlyDictionary<AutoInjectIdentifier, InjectorItem>.Empty,
            RandomStringGuid.Digits
        );
    }

    private ReadOnlyReadOnlyDependencyInjector CreateDependencyInjector(
        IReadOnlyDictionary<Type, InjectorItem> injectors,
        IReadOnlyDictionary<ReserveIdentifier, InjectorItem> reserves
    )
    {
        return CreateDependencyInjector(
            injectors,
            reserves,
            ReadOnlyDictionary<AutoInjectIdentifier, InjectorItem>.Empty,
            RandomStringGuid.Digits
        );
    }

    private ReadOnlyReadOnlyDependencyInjector CreateDependencyInjector(
        IReadOnlyDictionary<Type, InjectorItem> injectors,
        IReadOnlyDictionary<ReserveIdentifier, InjectorItem> reserves,
        IReadOnlyDictionary<AutoInjectIdentifier, InjectorItem> autoInjects
    )
    {
        return CreateDependencyInjector(injectors, reserves, autoInjects, RandomStringGuid.Digits);
    }

    private ReadOnlyReadOnlyDependencyInjector CreateDependencyInjector(
        IReadOnlyDictionary<Type, InjectorItem> injectors,
        IReadOnlyDictionary<ReserveIdentifier, InjectorItem> reserves,
        IReadOnlyDictionary<AutoInjectIdentifier, InjectorItem> autoInjects,
        IRandom<string> randomString
    )
    {
        return new ReadOnlyReadOnlyDependencyInjector(
            injectors,
            reserves,
            autoInjects,
            randomString
        );
    }

    private class ClassWithPropertySetter
    {
        public int Property { get; set; }
    }

    private class ClassWith2Constructors
    {
        private readonly int field;

        public ClassWith2Constructors() { }

        public ClassWith2Constructors(int field)
        {
            this.field = field;
        }
    }

    private class ClassWithoutConstructors
    {
        private ClassWithoutConstructors() { }
    }

    private struct StructWith1Constructor
    {
        public int Property { get; }

        public StructWith1Constructor(int property)
        {
            Property = property;
        }
    }

    private struct StructStructWith1ConstructorAndParameter
    {
        public int Property { get; }

        public StructStructWith1ConstructorAndParameter(int property)
        {
            Property = property;
        }
    }
}
