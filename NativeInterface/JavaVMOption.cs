using System;
using System.Runtime.InteropServices;

namespace NativeInterface
{
    [StructLayout(LayoutKind.Sequential)]
    public struct JavaVMOption
    {
        public IntPtr optionString;
        public IntPtr extraInfo;
    }
}
