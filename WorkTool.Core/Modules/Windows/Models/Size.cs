using System.Runtime.InteropServices;

namespace WorkTool.Core.Modules.Windows.Models;

[StructLayout(LayoutKind.Sequential)]
public struct Size
{
    public int X;
    public int Y;
}