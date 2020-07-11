using System;
using System.Runtime.InteropServices;

namespace NativeInterface
{
    public static class WindowsMethods
    {
        [DllImport("kernel32.dll")]
        internal static extern IntPtr LoadLibrary(string library);
    }
}
