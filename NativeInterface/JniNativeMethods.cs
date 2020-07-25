using System;
using System.Runtime.InteropServices;

namespace NativeInterface
{
    /*
     * The methods in the class follow the structure defined in the JavaSE JNI documentation
     * Available at: https://docs.oracle.com/javase/7/docs/technotes/guides/jni/spec/functions.html
     *
     * [UnmanagedFunctionPointer(CallingConvention.StdCall)] means that the callee cleans the stack
     * so that any objects and memory craeted will can be destroyed by the garbage collector
     */
    unsafe public class JniNativeMethods
    {
        // NativeType Call<type>MethodA(JNIEnv *env, jobject obj, jmethodID methodID, const jvalue* args)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void CallStaticVoidMethod(IntPtr env, IntPtr obj, IntPtr methodID, long* args);

        // NativeType Call<type>MethodA(JNIEnv* env, jobject obj, jmethodID methodID, const jvalue* args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void CallVoidMethodA(IntPtr env, IntPtr obj, IntPtr methodID, long* args);

        // jobject NewGlobalRef(JNIEnv *env, jobject obj);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate IntPtr NewGlobalRef(IntPtr env, IntPtr obj);

        // void DeleteLocalRef(JNIEnv *env, jobject localRef);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void DeleteLocalRef(IntPtr env, IntPtr localRef);

        // void DeleteGlobalRef(JNIEnv *env, jobject globalRef);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void DeleteGlobalRef(IntPtr env, IntPtr globalRef);

        // jclass GetObjectClass(JNIEnv *env, jobject obj);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate IntPtr GetObjectClass(IntPtr env, IntPtr obj);

        // jobject NewObjectA(JNIEnv *env, jclass clazz, jmethodID methodID, const jvalue* args);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate IntPtr NewObjectA(IntPtr env, IntPtr clazz, IntPtr methodID, long* args);

        // jclass FindClass(JNIEnv *env, const char *name);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate IntPtr FindClass(IntPtr env, [MarshalAs(UnmanagedType.LPStr)]string name);

        // jmethodID GetStaticMethodID(JNIEnv *env, jclass clazz, const char* name, const char* sig);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate IntPtr GetStaticMethodId(IntPtr env, IntPtr clazz,
            [MarshalAs(UnmanagedType.LPStr)]string name, [MarshalAs(UnmanagedType.LPStr)]string sig);

        // jmethodID GetMethodID(JNIEnv *env, jclass clazz, const char* name, const char* sig);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate IntPtr GetMethodID(IntPtr env, IntPtr clazz, [MarshalAs(UnmanagedType.LPStr)]string name,
            [MarshalAs(UnmanagedType.LPStr)]string sig);
        
        // jstring NewStringUTF(JNIEnv *env, const char *bytes);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate IntPtr NewStringUTF(IntPtr env, IntPtr bytes);
        
        // jthrowable ExceptionOccurred(JNIEnv *env);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate IntPtr ExceptionOccurred(IntPtr env);

        // void ExceptionClear(JNIEnv *env);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void ExceptionClear(IntPtr env);

        // jboolean ExceptionCheck(JNIEnv *env);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate byte ExceptionCheck(IntPtr env);

        // void ExceptionDescribe(JNIEnv *env);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void ExceptionDescribe(IntPtr env);

        // const jchar * GetStringChars(JNIEnv *env, jstring string, jboolean* isCopy);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate IntPtr GetStringChars(IntPtr env, IntPtr stringPtr, byte* isCopy);

        // void ReleaseStringChars(JNIEnv *env, jstring string, const jchar* chars);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void ReleaseStringChars(IntPtr env, IntPtr stringPtr, IntPtr chars);

        // const char * GetStringUTFChars(JNIEnv *env, jstring string, jboolean* isCopy);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate IntPtr GetStringUTFChars(IntPtr env, IntPtr stringPtr, byte* isCopy);

        // void ReleaseStringUTFChars(JNIEnv *env, jstring string, const char* utf);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void ReleaseStringUTFChars(IntPtr env, IntPtr stringPtr, IntPtr utf);

        // jsize GetStringUTFLength(JNIEnv *env, jstring string);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate int GetStringUTFLength(IntPtr env, IntPtr stringPtr);

        // jint ThrowNew(JNIEnv *env, jclass clazz, const char* message);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate int ThrowNew(IntPtr env, IntPtr clazz, IntPtr message);

        // jint RegisterNatives(JNIEnv *env, jclass clazz, const JNINativeMethod* methods, jint nMethods);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate int RegisterNatives(IntPtr env, IntPtr clazz, NativeJavaMethod* methods, int nMethods);

        internal FindClass findClass;
        internal NewGlobalRef newGlobalRef;
        internal DeleteLocalRef deleteLocalRef;
        internal DeleteGlobalRef deleteGlobalRef;
        internal GetObjectClass getObjectClass;
        internal NewObjectA newObject;
        internal NewStringUTF newStringUTF;
        internal GetStringChars getStringChars;
        internal ReleaseStringChars releaseStringChars;
        internal GetStringUTFChars getStringUTFChars;
        internal ReleaseStringUTFChars releaseStringUTFChars;
        internal GetStringUTFLength getStringUTFLength;
        internal RegisterNatives registerNatives;
        internal GetStaticMethodId getStaticMethodId;
        internal GetMethodID getMethodID;
        internal CallStaticVoidMethod callStaticVoidMethod;
        internal CallVoidMethodA callVoidMethod;
        internal ExceptionCheck exceptionCheck;
        internal ExceptionClear exceptionClear;
        internal ExceptionOccurred exceptionOccurred;
        internal ExceptionDescribe exceptionDescribe;

        public JniNativeMethods(IntPtr env)
        {
            JniNativeInterface javaNativeInterface = **(JniNativeInterface**)env;

            // Class delegates
            findClass = (FindClass)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.FindClass, typeof(FindClass));
            newGlobalRef = (NewGlobalRef)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.NewGlobalRef, typeof(NewGlobalRef));
            deleteLocalRef = (DeleteLocalRef)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.DeleteLocalRef, typeof(DeleteLocalRef));
            deleteGlobalRef = (DeleteGlobalRef)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.DeleteGlobalRef, typeof(DeleteGlobalRef));
            getObjectClass = (GetObjectClass)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.GetObjectClass, typeof(GetObjectClass));
            newObject = (NewObjectA)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.NewObjectA, typeof(NewObjectA));

            // String conversion delegates
            newStringUTF = (NewStringUTF)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.NewStringUTF, typeof(NewStringUTF));
            getStringChars = (GetStringChars)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.GetStringChars, typeof(GetStringChars));
            releaseStringChars = (ReleaseStringChars)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.ReleaseStringChars, typeof(ReleaseStringChars));
            getStringUTFChars = (GetStringUTFChars)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.GetStringUTFChars, typeof(GetStringUTFChars));
            releaseStringUTFChars = (ReleaseStringUTFChars)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.ReleaseStringUTFChars, typeof(ReleaseStringUTFChars));
            getStringUTFLength = (GetStringUTFLength)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.GetStringUTFLength, typeof(GetStringUTFLength));

            // Method delegates
            registerNatives = (RegisterNatives)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.RegisterNatives, typeof(RegisterNatives));
            getStaticMethodId = (GetStaticMethodId)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.GetStaticMethodID, typeof(GetStaticMethodId));
            getMethodID = (GetMethodID)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.GetMethodID, typeof(GetMethodID));
            callStaticVoidMethod = (CallStaticVoidMethod)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.CallStaticVoidMethod, typeof(CallStaticVoidMethod));
            callVoidMethod = (CallVoidMethodA)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.CallVoidMethodA, typeof(CallVoidMethodA));

            // Exception delegates
            exceptionCheck = (ExceptionCheck)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.ExceptionCheck, typeof(ExceptionCheck));
            exceptionOccurred = (ExceptionOccurred)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.ExceptionOccurred, typeof(ExceptionOccurred));
            exceptionClear = (ExceptionClear)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.ExceptionClear, typeof(ExceptionClear));
            exceptionDescribe = (ExceptionDescribe)Marshal.GetDelegateForFunctionPointer(javaNativeInterface.ExceptionDescribe, typeof(ExceptionDescribe));
        }
    }
}
