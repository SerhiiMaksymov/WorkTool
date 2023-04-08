namespace WorkTool.Core.Modules.Windows.Helpers;

public class ComGuids
{
    public const  string CLSID_ImmersiveShellStr                ="C2F03A33-21F5-47FA-B4BB-156362A2F239";
    public  const string CLSID_VirtualDesktopManagerInternalStr = "C5E0CDCA-7B6E-41B2-9FC4-D93975CC467B";
    public  const string CLSID_VirtualDesktopManagerStr         ="AA509086-5CA9-4C25-8F95-589D3C07B48A";
    public const  string CLSID_VirtualDesktopPinnedAppsStr      = "B5A399E7-1C87-46B8-88E9-FC5747B171BD";
    private const string AppOnAllDesktopsStr                    = "BB64D5B7-4DE3-4AB2-A87C-DB7601AEA7DC";
    private const string WindowOnAllDesktopsStr                 ="C2DDEA68-66F2-4CF9-8264-1BFD00FBBBAC";
    
    public static readonly Guid CLSID_ImmersiveShell                = new (CLSID_ImmersiveShellStr);
    public static readonly Guid CLSID_VirtualDesktopManagerInternal = new (CLSID_VirtualDesktopManagerInternalStr);
    public static readonly Guid CLSID_VirtualDesktopManager         = new (CLSID_VirtualDesktopManagerStr);
    public static readonly Guid CLSID_VirtualDesktopPinnedApps      = new (CLSID_VirtualDesktopPinnedAppsStr);
    public static readonly Guid AppOnAllDesktops               = new(AppOnAllDesktopsStr);
    public static readonly Guid WindowOnAllDesktops              =new(WindowOnAllDesktopsStr) ;
}