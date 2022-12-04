namespace WorkTool.SourceGenerator.Models;

public enum AccessModifier : byte
{
    NotApplicable,
    Public,
    Protected,
    Internal,
    ProtectedInternal,
    Private,
    PrivateProtected
}