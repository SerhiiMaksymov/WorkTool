namespace WorkTool.Core.NUnitTests.Modules.DependencyInjection.Services;

public class DependencyInjectorTests
{
    private static int structPropertyValue;

    private DependencyInjector? dependencyInjector;

    [SetUp]
    public void SetUp()
    {
        structPropertyValue = 15;
    }

    [Test]
    public void Inputs_RegisterDelegateWithParameterAndOutput_InputsEmpty()
    {
        var objectInjectorItem = new InjectorItem(
            InjectorItemType.Singleton,
            (int _) => new object()
        );

        var int32InjectorItem = new InjectorItem(
            InjectorItemType.Singleton,
            () => structPropertyValue
        );

        dependencyInjector = CreateDependencyInjector(
            new Dictionary<TypeInformation, InjectorItem>
            {
                { typeof(object), objectInjectorItem },
                { typeof(int), int32InjectorItem }
            }
        );

        dependencyInjector.Inputs.ToArray().Should().BeEmpty();
    }

    [Test]
    public void Inputs_RegisterAutoInjectDelegateWithParameter_InputWithParameterType()
    {
        var member = typeof(ClassWithPropertySetter)
            .GetMember(nameof(ClassWithPropertySetter.Property))
            .Single();

        var autoInjectMember = new AutoInjectMember(member);

        var autoInjectIdentifier = new AutoInjectMemberIdentifier(
            typeof(ClassWithPropertySetter),
            autoInjectMember
        );

        var injectorItem = new InjectorItem(
            InjectorItemType.Singleton,
            (int _) => new ClassWithPropertySetter()
        );

        dependencyInjector = CreateDependencyInjector(
            new Dictionary<AutoInjectMemberIdentifier, InjectorItem>
            {
                { autoInjectIdentifier, injectorItem }
            }
        );

        dependencyInjector.Inputs.ToArray().Should().HaveCount(1).And.Contain(typeof(int));
    }

    [Test]
    public void Inputs_RegisterDelegateWithParameter_InputWithParameterType()
    {
        var injectorItem = new InjectorItem(InjectorItemType.Singleton, (int _) => new object());

        var injectors = new Dictionary<TypeInformation, InjectorItem>
        {
            { typeof(object), injectorItem }
        };

        dependencyInjector = CreateDependencyInjector(injectors);

        dependencyInjector.Inputs.ToArray().Should().HaveCount(1).And.Contain(typeof(int));
    }

    [Test]
    public void Resolve_AutoProperty_SetProperty()
    {
        var type = typeof(ClassWithPropertySetter);
        var member = type.GetMember(nameof(ClassWithPropertySetter.Property))[0];

        dependencyInjector = CreateDependencyInjector(
            new Dictionary<TypeInformation, InjectorItem>
            {
                {
                    typeof(ClassWithPropertySetter),
                    new InjectorItem(
                        InjectorItemType.Singleton,
                        () => new ClassWithPropertySetter()
                    )
                }
            },
            new Dictionary<AutoInjectMemberIdentifier, InjectorItem>
            {
                {
                    new AutoInjectMemberIdentifier(type, member),
                    new InjectorItem(InjectorItemType.Singleton, () => structPropertyValue)
                }
            }
        );

        var value = dependencyInjector.Resolve(typeof(ClassWithPropertySetter));
        value.ThrowIfIsNot<ClassWithPropertySetter>().Property.Should().Be(structPropertyValue);
    }

    [Test]
    public void Resolve_UnCorrectInjectorItemType_Exception()
    {
        dependencyInjector = CreateDependencyInjector(
            new Dictionary<TypeInformation, InjectorItem>
            {
                { typeof(object), new InjectorItem((InjectorItemType)255, () => new object()) }
            }
        );

        var func = () => dependencyInjector.Resolve(typeof(object));
        func.Should().Throw<UnreachableException>();
    }

    [Test]
    public void Resolve_StructWith1Constructor_StructInstance()
    {
        dependencyInjector = CreateDependencyInjector(
            new Dictionary<TypeInformation, InjectorItem>
            {
                {
                    typeof(int),
                    new InjectorItem(InjectorItemType.Singleton, () => structPropertyValue)
                },
                {
                    typeof(StructWith1Constructor),
                    new InjectorItem(
                        InjectorItemType.Singleton,
                        (int value) => new StructWith1Constructor(value)
                    )
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
            new Dictionary<TypeInformation, InjectorItem>
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
            new Dictionary<TypeInformation, InjectorItem>
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
            new Dictionary<TypeInformation, InjectorItem>
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
            new Dictionary<TypeInformation, InjectorItem>
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
    public void Resolve_RegisterDelegateWithRecursion_Exception()
    {
        var func = () =>
            CreateDependencyInjector(
                new Dictionary<TypeInformation, InjectorItem>
                {
                    {
                        typeof(ClassWithoutConstructors),
                        new InjectorItem(
                            InjectorItemType.Transient,
                            (ClassWithoutConstructors cl) => cl
                        )
                    }
                }
            );

        func.Should()
            .Throw<RecursionTypeDelegateInvokeException>()
            .And.Type.Should()
            .Be(typeof(ClassWithoutConstructors));
    }

    private DependencyInjector CreateDependencyInjector()
    {
        return CreateDependencyInjector(
            ReadOnlyDictionary<TypeInformation, InjectorItem>.Empty,
            ReadOnlyDictionary<AutoInjectMemberIdentifier, InjectorItem>.Empty,
            ReadOnlyDictionary<TypeInformation, IEnumerable<InjectorItem>>.Empty,
            RandomStringGuid.Digits
        );
    }

    private DependencyInjector CreateDependencyInjector(
        IReadOnlyDictionary<TypeInformation, InjectorItem> injectors,
        IReadOnlyDictionary<AutoInjectMemberIdentifier, InjectorItem> autoInjectors
    )
    {
        return CreateDependencyInjector(
            injectors,
            autoInjectors,
            ReadOnlyDictionary<TypeInformation, IEnumerable<InjectorItem>>.Empty,
            RandomStringGuid.Digits
        );
    }

    private DependencyInjector CreateDependencyInjector(
        IReadOnlyDictionary<TypeInformation, IEnumerable<InjectorItem>> collections
    )
    {
        return CreateDependencyInjector(
            ReadOnlyDictionary<TypeInformation, InjectorItem>.Empty,
            ReadOnlyDictionary<AutoInjectMemberIdentifier, InjectorItem>.Empty,
            collections,
            RandomStringGuid.Digits
        );
    }

    private DependencyInjector CreateDependencyInjector(
        IReadOnlyDictionary<AutoInjectMemberIdentifier, InjectorItem> autoInjects,
        IReadOnlyDictionary<TypeInformation, IEnumerable<InjectorItem>> collections
    )
    {
        return CreateDependencyInjector(
            ReadOnlyDictionary<TypeInformation, InjectorItem>.Empty,
            autoInjects,
            collections,
            RandomStringGuid.Digits
        );
    }

    private DependencyInjector CreateDependencyInjector(
        IReadOnlyDictionary<AutoInjectMemberIdentifier, InjectorItem> autoInjects
    )
    {
        return CreateDependencyInjector(
            ReadOnlyDictionary<TypeInformation, InjectorItem>.Empty,
            autoInjects,
            ReadOnlyDictionary<TypeInformation, IEnumerable<InjectorItem>>.Empty,
            RandomStringGuid.Digits
        );
    }

    private DependencyInjector CreateDependencyInjector(
        IReadOnlyDictionary<TypeInformation, InjectorItem> injectors,
        IReadOnlyDictionary<TypeInformation, IEnumerable<InjectorItem>> collections
    )
    {
        return CreateDependencyInjector(
            injectors,
            ReadOnlyDictionary<AutoInjectMemberIdentifier, InjectorItem>.Empty,
            collections,
            RandomStringGuid.Digits
        );
    }

    private DependencyInjector CreateDependencyInjector(
        IReadOnlyDictionary<TypeInformation, InjectorItem> injectors
    )
    {
        return CreateDependencyInjector(
            injectors,
            ReadOnlyDictionary<AutoInjectMemberIdentifier, InjectorItem>.Empty,
            ReadOnlyDictionary<TypeInformation, IEnumerable<InjectorItem>>.Empty,
            RandomStringGuid.Digits
        );
    }

    private DependencyInjector CreateDependencyInjector(
        IReadOnlyDictionary<TypeInformation, InjectorItem> injectors,
        IReadOnlyDictionary<AutoInjectMemberIdentifier, InjectorItem> autoInjects,
        IReadOnlyDictionary<TypeInformation, IEnumerable<InjectorItem>> collections
    )
    {
        return CreateDependencyInjector(
            injectors,
            autoInjects,
            collections,
            RandomStringGuid.Digits
        );
    }

    private DependencyInjector CreateDependencyInjector(
        IReadOnlyDictionary<TypeInformation, InjectorItem> injectors,
        IReadOnlyDictionary<AutoInjectMemberIdentifier, InjectorItem> autoInjects,
        IReadOnlyDictionary<TypeInformation, IEnumerable<InjectorItem>> collections,
        IRandom<string> randomString
    )
    {
        return new DependencyInjector(injectors, autoInjects, collections, randomString);
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
