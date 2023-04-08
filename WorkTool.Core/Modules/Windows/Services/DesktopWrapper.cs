using System.Runtime.InteropServices;

using WorkTool.Core.Modules.Windows.Helpers;
using WorkTool.Core.Modules.Windows.Interfaces;

namespace WorkTool.Core.Modules.Windows.Services;

public class DesktopWrapper
{
    public readonly static DesktopWrapper Instance = new();
    
    private DesktopWrapper()
    {
        var shell = (IServiceProvider10)Activator.CreateInstance(Type.GetTypeFromCLSID(ComGuids.CLSID_ImmersiveShell));

        VirtualDesktopManagerInternal = (IVirtualDesktopManagerInternal)shell.QueryService(
            ComGuids.CLSID_VirtualDesktopManagerInternal,
            typeof(IVirtualDesktopManagerInternal).GUID);

        VirtualDesktopManager =
            (IVirtualDesktopManager)Activator.CreateInstance(Type.GetTypeFromCLSID(ComGuids.CLSID_VirtualDesktopManager));

        ApplicationViewCollection = (IApplicationViewCollection)shell.QueryService(
            typeof(IApplicationViewCollection).GUID,
            typeof(IApplicationViewCollection).GUID);

        VirtualDesktopPinnedApps = (IVirtualDesktopPinnedApps)shell.QueryService(
            ComGuids.CLSID_VirtualDesktopPinnedApps,
            typeof(IVirtualDesktopPinnedApps).GUID);
    }

    public IVirtualDesktopManagerInternal VirtualDesktopManagerInternal;
    public   IVirtualDesktopManager         VirtualDesktopManager;
    public   IApplicationViewCollection     ApplicationViewCollection;
    public   IVirtualDesktopPinnedApps      VirtualDesktopPinnedApps;

    public IVirtualDesktop GetDesktop(int index)
    {
        // get desktop with index
        int count = VirtualDesktopManagerInternal.GetCount(IntPtr.Zero);

        if (index < 0 || index >= count) throw new ArgumentOutOfRangeException("index");

        IObjectArray desktops;
        VirtualDesktopManagerInternal.GetDesktops(IntPtr.Zero, out desktops);
        object objdesktop;
        desktops.GetAt(index, typeof(IVirtualDesktop).GUID, out objdesktop);
        Marshal.ReleaseComObject(desktops);

        return (IVirtualDesktop)objdesktop;
    }

    public  int GetDesktopIndex(IVirtualDesktop desktop)
    {
        // get index of desktop
        int          index    = -1;
        Guid         IdSearch = desktop.GetId();
        IObjectArray desktops;
        VirtualDesktopManagerInternal.GetDesktops(IntPtr.Zero, out desktops);
        object objdesktop;

        for (int i = 0; i < VirtualDesktopManagerInternal.GetCount(IntPtr.Zero); i++)
        {
            desktops.GetAt(i, typeof(IVirtualDesktop).GUID, out objdesktop);

            if (IdSearch.CompareTo(((IVirtualDesktop)objdesktop).GetId()) == 0)
            {
                index = i;

                break;
            }
        }

        Marshal.ReleaseComObject(desktops);

        return index;
    }
    
    public  IApplicationView GetApplicationView(IntPtr hWnd)
    {
        // get application view to window handle
        IApplicationView view;
        ApplicationViewCollection.GetViewForHwnd(hWnd, out view);

        return view;
    }

    public  string GetAppId(IntPtr hWnd)
    {
        // get Application ID to window handle
        string appId;
        GetApplicationView(hWnd).GetAppUserModelId(out appId);

        return appId;
    }
}