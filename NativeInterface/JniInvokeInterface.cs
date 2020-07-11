using System;
using System.Runtime.InteropServices;

namespace NativeInterface
{
    // https://docs.oracle.com/javase/7/docs/technotes/guides/jni/spec/invocation.html
    // Struct layout is important here to preserve the location of each pointer in bytes
    [StructLayout(LayoutKind.Sequential)]
    public struct JniInvokeInterface
    {
        public IntPtr NULL0;
        public IntPtr NULL1;
        public IntPtr NULL2;

        public IntPtr DestroyJavaVM;
        public IntPtr AttachCurrentThread;
        public IntPtr DetachCurrentThread;

        public IntPtr GetEnv;

        public IntPtr AttachCurrentThreadAsDaemo;
    }
}
