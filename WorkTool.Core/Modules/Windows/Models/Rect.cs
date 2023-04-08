using System.Runtime.InteropServices;

namespace WorkTool.Core.Modules.Windows.Models;

[StructLayout(LayoutKind.Sequential)]
public struct Rect
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;
}