using System.Runtime.InteropServices;

namespace WorkTool.Core.Modules.Windows.Helpers;

public static class Interop
{
    [DllImport("user32.dll")]
    public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();
}