namespace WorkTool.Core.Modules.Graph.Models;

public class TreeVerticalStringHumanizingOptions
{
    public ushort FirstIndent { get; set; }
    public ushort Indent { get; set; }
    public string Format { get; set; }

    public TreeVerticalStringHumanizingOptions()
    {
        Indent = 1;
        Format = "{0}{1}";
    }
}
