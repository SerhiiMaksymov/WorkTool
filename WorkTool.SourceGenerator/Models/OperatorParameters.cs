namespace WorkTool.SourceGenerator.Models;

public readonly struct OperatorParameters
{
    public OperatorParameters(AccessModifier access, OperatorType type, TypeParameters output, ArgumentParameters input,
                              string         body)
    {
        Access = access;
        Type   = type;
        Output = output;
        Input  = input;
        Body   = body;
    }

    public AccessModifier     Access { get; }
    public OperatorType       Type   { get; }
    public TypeParameters     Output { get; }
    public ArgumentParameters Input  { get; }
    public string             Body   { get; }

    public override string ToString()
    {
        return $@"{
            Access.AsString()
        } static {
            Type.AsString()
        } operator {
            Output
        }({
            Input
        })
{{
    {
        Body
    }  
}}";
    }
}