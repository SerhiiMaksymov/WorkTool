using System.Runtime.InteropServices;

namespace WorkTool.Core.Modules.Windows.Interfaces;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("A5CD92FF-29BE-454C-8D04-D82879FB3F1B")]
public interface IVirtualDesktopManager
{
    bool IsWindowOnCurrentVirtualDesktop(IntPtr topLevelWindow);
    Guid GetWindowDesktopId(IntPtr              topLevelWindow);
    void MoveWindowToDesktop(IntPtr             topLevelWindow, ref Guid desktopId);
}