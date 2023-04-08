using System.Runtime.InteropServices;

namespace WorkTool.Core.Modules.Windows.Interfaces;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("4CE81583-1E4C-4632-A621-07A53543148F")]
public interface IVirtualDesktopPinnedApps
{
    bool IsAppIdPinned(string          appId);
    void PinAppID(string               appId);
    void UnpinAppID(string             appId);
    bool IsViewPinned(IApplicationView applicationView);
    void PinView(IApplicationView      applicationView);
    void UnpinView(IApplicationView    applicationView);
}