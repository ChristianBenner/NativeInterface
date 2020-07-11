using System.Runtime.InteropServices;

namespace NativeInterface
{
    [StructLayout(LayoutKind.Sequential)]
    public struct JavaVMInitArgs
    {
        public int version;
        public int nOptions;
        public unsafe JavaVMOption* options;
        public readonly byte ignoreUnrecognised;
    }
}
