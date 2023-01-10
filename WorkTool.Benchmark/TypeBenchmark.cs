namespace WorkTool.Benchmark;

public class TypeBenchmark
{
    private readonly Type type1 = typeof(Class1);
    private readonly Type type2 = typeof(Class2);
    private readonly TypeIdentifier typeIdentifier1 = new(typeof(Class1));
    private readonly TypeIdentifier typeIdentifier2 = new(typeof(Class2));

    [Benchmark]
    public bool TypeIdentifierEquals()
    {
        return typeIdentifier1.Equals(typeIdentifier2);
    }

    [Benchmark]
    public bool TypeEquals()
    {
        return type1 == type2;
    }

    private class Class1 { }

    private class Class2 { }
}
