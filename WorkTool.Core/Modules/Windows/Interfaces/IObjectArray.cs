using System.Runtime.InteropServices;

namespace WorkTool.Core.Modules.Windows.Interfaces;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("92CA9DCD-5622-4BBA-A805-5E9F541BD8C9")]
public interface IObjectArray
{
    void GetCount(out int count);
    void GetAt(int        index, ref Guid iid, [MarshalAs(UnmanagedType.Interface)]out object obj);
}