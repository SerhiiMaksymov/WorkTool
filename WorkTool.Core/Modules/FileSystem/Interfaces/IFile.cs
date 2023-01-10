namespace WorkTool.Core.Modules.FileSystem.Interfaces;

public interface IFile
{
    QuantitiesInformation Size { get; }
    IDirectory?            Directory { get; }
    FileName              FileName  { get; }
}
