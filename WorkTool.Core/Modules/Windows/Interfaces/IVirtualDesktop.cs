using System.Runtime.InteropServices;

namespace WorkTool.Core.Modules.Windows.Interfaces;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("536D3495-B208-4CC9-AE26-DE8111275BF8")]
public interface IVirtualDesktop
{
    bool   IsViewVisible(IApplicationView view);
    Guid   GetId();
    IntPtr Unknown1();
    [return: MarshalAs(UnmanagedType.HString)]
    string GetName();
    [return: MarshalAs(UnmanagedType.HString)]
    string GetWallpaperPath();
}