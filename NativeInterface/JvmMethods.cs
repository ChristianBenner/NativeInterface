using System;
using System.Runtime.InteropServices;

namespace NativeInterface
{
    // https://docs.oracle.com/javase/7/docs/technotes/guides/jni/spec/invocation.html
    public static unsafe class JvmMethods
    {
        // void JNI_DestroyJavaVM();
        [DllImport("jvm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern void JNI_DestroyJavaVM();

        // jint JNI_CreateJavaVM(JavaVM **p_vm, void **p_env, void *vm_args);
        [DllImport("jvm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern int JNI_CreateJavaVM(out IntPtr p_vm, out IntPtr p_env, JavaVMInitArgs* vm_args);

        // jint JNI_GetCreatedJavaVMs(JavaVM **vmBuf, jsize bufLen, jsize *nVMs);
        [DllImport("jvm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern int JNI_GetCreatedJavaVMs(out IntPtr vmBuf, int bufLen, [Out]out int nVMs);

        // jint JNI_GetDefaultJavaVMInitArgs(void *vm_args);
        [DllImport("jvm.dll", CallingConvention = CallingConvention.StdCall)]
        internal static extern int JNI_GetDefaultJavaVMInitArgs(JavaVMInitArgs* vm_args);
    }
}
