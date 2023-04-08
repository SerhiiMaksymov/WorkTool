using WorkTool.Core.Modules.Windows.Helpers;
using WorkTool.Core.Modules.Windows.Interfaces;

namespace WorkTool.Core.Modules.Windows.Services;

public class DesktopManager
{
    private          IVirtualDesktop ivd;
    private readonly DesktopWrapper  desktopWrapper;

    public DesktopManager(IVirtualDesktop desktop, DesktopWrapper wrapper)
    {
        desktopWrapper = wrapper;
        this.ivd       = desktop;
    }

    public override int GetHashCode()
    {
        // get hash
        return ivd.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        // compare with object
        var desk = obj as DesktopManager;

        return desk != null && object.ReferenceEquals(this.ivd, desk.ivd);
    }

    public int Count
    {
        // return the number of desktops
        get { return desktopWrapper.VirtualDesktopManagerInternal.GetCount(IntPtr.Zero); }
    }

    public static DesktopManager Current
    {
        // returns current desktop
        get
        {
            return new DesktopManager(DesktopWrapper.Instance.VirtualDesktopManagerInternal.GetCurrentDesktop(IntPtr.Zero), DesktopWrapper.Instance);
        }
    }

    public static DesktopManager FromIndex(int index)
    { // return desktop object from index (-> index = 0..Count-1)
        return new DesktopManager(DesktopWrapper.Instance.GetDesktop(index), DesktopWrapper.Instance);
    }

    public static DesktopManager FromWindow(IntPtr hWnd)
    {
        // return desktop object to desktop on which window <hWnd> is displayed
        if (hWnd == IntPtr.Zero) throw new ArgumentNullException();

        Guid id = DesktopWrapper.Instance.VirtualDesktopManager.GetWindowDesktopId(hWnd);

        if ((id.CompareTo(ComGuids.AppOnAllDesktops) == 0) || (id.CompareTo(ComGuids.WindowOnAllDesktops) == 0))
            return new DesktopManager(DesktopWrapper.Instance.VirtualDesktopManagerInternal.GetCurrentDesktop(IntPtr.Zero), DesktopWrapper.Instance);
        else
            return new DesktopManager(DesktopWrapper.Instance.VirtualDesktopManagerInternal.FindDesktop(ref id), DesktopWrapper.Instance);
    }

    public int FromDesktop(DesktopManager desktop)
    {
        // return index of desktop object or -1 if not found
        return desktopWrapper.GetDesktopIndex(desktop.ivd);
    }

    public string DesktopNameFromDesktop(DesktopManager desktop)
    {
        // return name of desktop or "DesktopManager n" if it has no name

        // get desktop name
        string desktopName = null;

        try
        {
            desktopName = desktop.ivd.GetName();
        }
        catch
        {
        }

        // no name found, generate generic name
        if (string.IsNullOrEmpty(desktopName))
        {
            // create name "DesktopManager n" (n = number starting with 1)
            desktopName = "DesktopManager " + (desktopWrapper.GetDesktopIndex(desktop.ivd) + 1).ToString();
        }

        return desktopName;
    }

    public string DesktopNameFromIndex(int index)
    {
        // return name of desktop from index (-> index = 0..Count-1) or "DesktopManager n" if it has no name

        // get desktop name
        string desktopName = null;

        try
        {
            desktopName = desktopWrapper.GetDesktop(index).GetName();
        }
        catch
        {
        }

        // no name found, generate generic name
        if (string.IsNullOrEmpty(desktopName))
        {
            // create name "DesktopManager n" (n = number starting with 1)
            desktopName = "DesktopManager " + (index + 1).ToString();
        }

        return desktopName;
    }

    public bool HasDesktopNameFromIndex(int index)
    {
        // return true is desktop is named or false if it has no name

        // read desktop name in registry
        string desktopName = null;

        try
        {
            desktopName = desktopWrapper.GetDesktop(index).GetName();
        }
        catch
        {
        }

        // name found?
        if (string.IsNullOrEmpty(desktopName))
            return false;
        else
            return true;
    }

    public string DesktopWallpaperFromIndex(int index)
    {
        // return name of desktop wallpaper from index (-> index = 0..Count-1)

        // get desktop name
        string desktopwppath = "";

        try
        {
            desktopwppath = desktopWrapper.GetDesktop(index).GetWallpaperPath();
        }
        catch
        {
        }

        return desktopwppath;
    }

    public int SearchDesktop(string partialName)
    {
        // get index of desktop with partial name, return -1 if no desktop found
        int index = -1;

        for (int i = 0; i < desktopWrapper.VirtualDesktopManagerInternal.GetCount(IntPtr.Zero); i++)
        {
            // loop through all virtual desktops and compare partial name to desktop name
            if (DesktopNameFromIndex(i).ToUpper().IndexOf(partialName.ToUpper()) >= 0)
            {
                index = i;

                break;
            }
        }

        return index;
    }

    public DesktopManager Create()
    {
        var desktop = desktopWrapper.VirtualDesktopManagerInternal.CreateDesktop(IntPtr.Zero);
        // create a new desktop
        return new DesktopManager(desktop, desktopWrapper);
    }

    public void Remove(DesktopManager fallback = null)
    {
        // destroy desktop and switch to <fallback>
        IVirtualDesktop fallbackdesktop;

        if (fallback == null)
        {
            // if no fallback is given use desktop to the left except for desktop 0.
            DesktopManager dtToCheck = new DesktopManager(desktopWrapper.GetDesktop(0), desktopWrapper);

            if (this.Equals(dtToCheck))
            {
                // desktop 0: set fallback to second desktop (= "right" desktop)
                desktopWrapper.VirtualDesktopManagerInternal.GetAdjacentDesktop(
                    ivd,
                    4,
                    out fallbackdesktop); // 4 = RightDirection
            }
            else
            {
                // set fallback to "left" desktop
                desktopWrapper.VirtualDesktopManagerInternal.GetAdjacentDesktop(
                    ivd,
                    3,
                    out fallbackdesktop); // 3 = LeftDirection
            }
        }
        else
            // set fallback desktop
            fallbackdesktop = fallback.ivd;

        desktopWrapper.VirtualDesktopManagerInternal.RemoveDesktop(ivd, fallbackdesktop);
    }

    public void RemoveAll()
    {
        // remove all desktops but visible
        desktopWrapper.VirtualDesktopManagerInternal.SetDesktopIsPerMonitor(true);
    }

    public void Move(int index)
    {
        // move current desktop to desktop in index (-> index = 0..Count-1)
        desktopWrapper.VirtualDesktopManagerInternal.MoveDesktop(ivd, IntPtr.Zero, index);
    }

    public void SetName(string Name)
    {
        // set name for desktop, empty string removes name
        desktopWrapper.VirtualDesktopManagerInternal.SetDesktopName(this.ivd, Name);
    }

    public void SetWallpaperPath(string Path)
    {
        // set path for wallpaper, empty string removes path
        if (string.IsNullOrEmpty(Path)) throw new ArgumentNullException();

        desktopWrapper.VirtualDesktopManagerInternal.SetDesktopWallpaper(this.ivd, Path);
    }

    public void SetAllWallpaperPaths(string Path)
    {
        // set wallpaper path for all desktops
        if (string.IsNullOrEmpty(Path)) throw new ArgumentNullException();

        desktopWrapper.VirtualDesktopManagerInternal.UpdateWallpaperPathForAllDesktops(Path);
    }

    public bool IsVisible
    {
        // return true if this desktop is the current displayed one
        get
        {
            return object.ReferenceEquals(
                ivd,
                desktopWrapper.VirtualDesktopManagerInternal.GetCurrentDesktop(IntPtr.Zero));
        }
    }

    public void MakeVisible()
    {
        // make this desktop visible
        desktopWrapper.VirtualDesktopManagerInternal.SwitchDesktop(IntPtr.Zero, ivd);
    }

    public DesktopManager Left
    {
        // return desktop at the left of this one, null if none
        get
        {
            IVirtualDesktop desktop;

            int hr = desktopWrapper.VirtualDesktopManagerInternal.GetAdjacentDesktop(
                ivd,
                3,
                out desktop); // 3 = LeftDirection

            if (hr == 0)
                return new DesktopManager(desktop, desktopWrapper);
            else
                return null;
        }
    }

    public DesktopManager Right
    {
        // return desktop at the right of this one, null if none
        get
        {
            IVirtualDesktop desktop;

            int hr = desktopWrapper.VirtualDesktopManagerInternal.GetAdjacentDesktop(
                ivd,
                4,
                out desktop); // 4 = RightDirection

            if (hr == 0)
                return new DesktopManager(desktop, desktopWrapper);
            else
                return null;
        }
    }

    public void MoveWindow(IntPtr hWnd)
    {
        // move window to this desktop
        int processId;

        if (hWnd == IntPtr.Zero) throw new ArgumentNullException();

        Interop.GetWindowThreadProcessId(hWnd, out processId);

        if (System.Diagnostics.Process.GetCurrentProcess().Id == processId)
        {
            // window of process
            try // the easy way (if we are owner)
            {
                desktopWrapper.VirtualDesktopManager.MoveWindowToDesktop(hWnd, ivd.GetId());
            }
            catch // window of process, but we are not the owner
            {
                IApplicationView view;
                desktopWrapper.ApplicationViewCollection.GetViewForHwnd(hWnd, out view);
                desktopWrapper.VirtualDesktopManagerInternal.MoveViewToDesktop(view, ivd);
            }
        }
        else
        {
            // window of other process
            IApplicationView view;
            desktopWrapper.ApplicationViewCollection.GetViewForHwnd(hWnd, out view);

            try
            {
                desktopWrapper.VirtualDesktopManagerInternal.MoveViewToDesktop(view, ivd);
            }
            catch
            {
                // could not move active window, try main window (or whatever windows thinks is the main window)
                desktopWrapper.ApplicationViewCollection.GetViewForHwnd(
                    System.Diagnostics.Process.GetProcessById(processId).MainWindowHandle,
                    out view);

                desktopWrapper.VirtualDesktopManagerInternal.MoveViewToDesktop(view, ivd);
            }
        }
    }

    public void MoveActiveWindow()
    {
        // move active window to this desktop
        MoveWindow(Interop.GetForegroundWindow());
    }

    public bool HasWindow(IntPtr hWnd)
    {
        // return true if window is on this desktop
        if (hWnd == IntPtr.Zero) throw new ArgumentNullException();

        Guid id = desktopWrapper.VirtualDesktopManager.GetWindowDesktopId(hWnd);

        if ((id.CompareTo(ComGuids.AppOnAllDesktops) == 0) || (id.CompareTo(ComGuids.WindowOnAllDesktops) == 0))
            return true;
        else
            return ivd.GetId() == id;
    }

    public bool IsWindowPinned(IntPtr hWnd)
    {
        // return true if window is pinned to all desktops
        if (hWnd == IntPtr.Zero) throw new ArgumentNullException();

        return desktopWrapper.VirtualDesktopPinnedApps.IsViewPinned(desktopWrapper.GetApplicationView(hWnd));
    }

    public void PinWindow(IntPtr hWnd)
    {
        // pin window to all desktops
        if (hWnd == IntPtr.Zero) throw new ArgumentNullException();

        var view = desktopWrapper.GetApplicationView(hWnd);

        if (!desktopWrapper.VirtualDesktopPinnedApps.IsViewPinned(view))
        {
            // pin only if not already pinned
            desktopWrapper.VirtualDesktopPinnedApps.PinView(view);
        }
    }

    public void UnpinWindow(IntPtr hWnd)
    {
        // unpin window from all desktops
        if (hWnd == IntPtr.Zero) throw new ArgumentNullException();

        var view = desktopWrapper.GetApplicationView(hWnd);

        if (desktopWrapper.VirtualDesktopPinnedApps.IsViewPinned(view))
        {
            // unpin only if not already unpinned
            desktopWrapper.VirtualDesktopPinnedApps.UnpinView(view);
        }
    }

    public bool IsApplicationPinned(IntPtr hWnd)
    {
        // return true if application for window is pinned to all desktops
        if (hWnd == IntPtr.Zero) throw new ArgumentNullException();

        return desktopWrapper.VirtualDesktopPinnedApps.IsAppIdPinned(desktopWrapper.GetAppId(hWnd));
    }

    public void PinApplication(IntPtr hWnd)
    {
        // pin application for window to all desktops
        if (hWnd == IntPtr.Zero) throw new ArgumentNullException();

        string appId = desktopWrapper.GetAppId(hWnd);

        if (!desktopWrapper.VirtualDesktopPinnedApps.IsAppIdPinned(appId))
        {
            // pin only if not already pinned
            desktopWrapper.VirtualDesktopPinnedApps.PinAppID(appId);
        }
    }

    public void UnpinApplication(IntPtr hWnd)
    {
        // unpin application for window from all desktops
        if (hWnd == IntPtr.Zero) throw new ArgumentNullException();

        var    view  = desktopWrapper.GetApplicationView(hWnd);
        string appId = desktopWrapper.GetAppId(hWnd);

        if (desktopWrapper.VirtualDesktopPinnedApps.IsAppIdPinned(appId))
        {
            // unpin only if pinned
            desktopWrapper.VirtualDesktopPinnedApps.UnpinAppID(appId);
        }
    }
}