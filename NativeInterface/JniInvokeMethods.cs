using System;
using System.Runtime.InteropServices;

namespace NativeInterface
{
    unsafe public class JniInvokeMethods
    {
        // jint AttachCurrentThread(JavaVM *vm, void **p_env, void *thr_args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate int AttachCurrentThread(IntPtr vm, out IntPtr p_env, IntPtr thr_args);

        // jint DestroyJavaVM(JavaVM *vm);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate int DestroyJavaVM(IntPtr vm);

        // jint DetachCurrentThread(JavaVM *vm);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate int DetachCurrentThread(IntPtr vm);

        internal AttachCurrentThread attachCurrentThread;
        internal DetachCurrentThread detachCurrentThread;
        internal DestroyJavaVM destroyJavaVM;

        public JniInvokeMethods(IntPtr jvm)
        {
            JniInvokeInterface func = **(JniInvokeInterface**)jvm;
            attachCurrentThread = (AttachCurrentThread)Marshal.GetDelegateForFunctionPointer(func.AttachCurrentThread, typeof(AttachCurrentThread));
            detachCurrentThread = (DetachCurrentThread)Marshal.GetDelegateForFunctionPointer(func.DetachCurrentThread, typeof(DetachCurrentThread));
            destroyJavaVM = (DestroyJavaVM)Marshal.GetDelegateForFunctionPointer(func.DestroyJavaVM, typeof(DestroyJavaVM));
        }
    }
}
