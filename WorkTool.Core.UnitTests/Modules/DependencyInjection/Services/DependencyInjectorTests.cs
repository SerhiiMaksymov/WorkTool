namespace WorkTool.Core.UnitTests.Modules.DependencyInjection.Services;

public class DependencyInjectorTests
{
    private static int structPropertyValue;

    private DependencyInjector? dependencyInjector;

    [SetUp]
    public void SetUp()
    {
        structPropertyValue = 15;

        dependencyInjector = new DependencyInjector(
            new Dictionary<Type, InjectorItem>(),
            new Dictionary<ReserveIdentifier, InjectorItem>(),
            new Dictionary<AutoInjectIdentifier, InjectorItem>()
        );
    }

    [Test]
    public void Resolve_UnCorrectInjectorItemType_Exception()
    {
        dependencyInjector = new DependencyInjector(
            new Dictionary<Type, InjectorItem>()
            {
                { typeof(object), new InjectorItem((InjectorItemType)255, () => new object()) }
            },
            new Dictionary<ReserveIdentifier, InjectorItem>(),
            new Dictionary<AutoInjectIdentifier, InjectorItem>()
        );

        var func = () => dependencyInjector.Resolve(typeof(object));

        func.Should().Throw<UnreachableException>();
    }

    [Test]
    public void Resolve_StructWithoutConstructor_StructInstance()
    {
        dependencyInjector = dependencyInjector.ThrowIfNull();

        var instance = dependencyInjector.Resolve(typeof(int));

        var int32 = instance.ThrowIfIsNot<int>();
        int32.Should().Be(0);
    }

    [Test]
    public void Resolve_StructWith1Constructor_StructInstance()
    {
        dependencyInjector = dependencyInjector.ThrowIfNull();

        var instance = dependencyInjector.Resolve(typeof(StructWith1Constructor));

        var value = instance.ThrowIfIsNot<StructWith1Constructor>();
        value.Property.Should().Be(structPropertyValue);
    }

    [Test]
    public void Resolve_StructWith1ConstructorAndParameter_StructInstance()
    {
        dependencyInjector = new DependencyInjector(
            new Dictionary<Type, InjectorItem>()
            {
                {
                    typeof(StructStructWith1ConstructorAndParameter),
                    new InjectorItem(
                        InjectorItemType.Transient,
                        () => new StructStructWith1ConstructorAndParameter(structPropertyValue)
                    )
                }
            },
            new Dictionary<ReserveIdentifier, InjectorItem>(),
            new Dictionary<AutoInjectIdentifier, InjectorItem>()
        );

        var instance = dependencyInjector.Resolve(typeof(StructStructWith1ConstructorAndParameter));

        var value = instance.ThrowIfIsNot<StructStructWith1ConstructorAndParameter>();
        value.Property.Should().Be(structPropertyValue);
    }

    [Test]
    public void Resolve_SingletonLifeCircle_SingleObject()
    {
        dependencyInjector = new DependencyInjector(
            new Dictionary<Type, InjectorItem>()
            {
                { typeof(object), new InjectorItem(InjectorItemType.Singleton, () => new object()) }
            },
            new Dictionary<ReserveIdentifier, InjectorItem>(),
            new Dictionary<AutoInjectIdentifier, InjectorItem>()
        );

        var instance1 = dependencyInjector.Resolve(typeof(object));
        var instance2 = dependencyInjector.Resolve(typeof(object));

        instance1.Should().Be(instance2);
    }

    [Test]
    public void Resolve_TransientLifeCircle_NewObject()
    {
        dependencyInjector = new DependencyInjector(
            new Dictionary<Type, InjectorItem>()
            {
                {
                    typeof(object),
                    new InjectorItem(InjectorItemType.Transient, () => new object())
                },
            },
            new Dictionary<ReserveIdentifier, InjectorItem>(),
            new Dictionary<AutoInjectIdentifier, InjectorItem>()
        );

        var instance1 = dependencyInjector.Resolve(typeof(object));
        var instance2 = dependencyInjector.Resolve(typeof(object));

        instance1.Should().NotBe(instance2);
    }

    [Test]
    public void Resolve_UseFunc_Instance()
    {
        dependencyInjector = new DependencyInjector(
            new Dictionary<Type, InjectorItem>()
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
            },
            new Dictionary<ReserveIdentifier, InjectorItem>(),
            new Dictionary<AutoInjectIdentifier, InjectorItem>()
        );

        var instance = dependencyInjector.Resolve(typeof(StructStructWith1ConstructorAndParameter));

        var value = instance.ThrowIfIsNot<StructStructWith1ConstructorAndParameter>();
        value.Property.Should().Be(structPropertyValue);
    }

    [Test]
    public void Resolve_ClassWithoutConstructors_Exception()
    {
        dependencyInjector = dependencyInjector.ThrowIfNull();

        var func = () => dependencyInjector.Resolve(typeof(ClassWithoutConstructors));

        func.Should()
            .Throw<NotHaveConstructorException>()
            .And.Type.Should()
            .Be(typeof(ClassWithoutConstructors));
    }

    [Test]
    public void Resolve_Class2Constructors_Exception()
    {
        dependencyInjector = dependencyInjector.ThrowIfNull();

        var func = () => dependencyInjector.Resolve(typeof(ClassWith2Constructors));

        func.Should()
            .Throw<ToManyConstructorsException>()
            .And.Type.Should()
            .Be(typeof(ClassWith2Constructors));
    }

    public class ClassWith2Constructors
    {
        public ClassWith2Constructors() { }

        public ClassWith2Constructors(int value) { }
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
            Property = structPropertyValue;
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
