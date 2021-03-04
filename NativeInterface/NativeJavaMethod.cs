using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NativeInterface
{
    public struct NativeJavaMethod
    {
        public IntPtr name;
        public IntPtr signature;
        public IntPtr funcPtr;

        public static unsafe NativeJavaMethod DescribeNativeMethod(string name,
            string signature,
            Delegate methodDelegate)
        {
            return new NativeJavaMethod
            {
                name = (IntPtr)convertStringToUtf8(name),
                signature = (IntPtr)convertStringToUtf8(signature),
                funcPtr = Marshal.GetFunctionPointerForDelegate(methodDelegate)
            };
        }

        public static unsafe NativeJavaMethod DescribeNativeVoidMethod(string name,
            Delegate methodDelegate)
        {
            return new NativeJavaMethod
            {
                name = (IntPtr)convertStringToUtf8(name),
                signature = (IntPtr)convertStringToUtf8("()V"),
                funcPtr = Marshal.GetFunctionPointerForDelegate(methodDelegate)
            };
        }

        // Converts to UTF-8 string and adds a null terminator
        private unsafe static sbyte* convertStringToUtf8(string theString)
        {
            if(theString == null)
            {
                return (sbyte*)IntPtr.Zero.ToPointer();
            }
            else
            {
                byte[] bytes = Encoding.UTF8.GetBytes(theString);

                // Allocated the length of the string (+1 for the null term char) to pointer loc
                IntPtr memory = Marshal.AllocHGlobal(bytes.Length + 1);

                // Copy bytes to location in memory
                Marshal.Copy(bytes, 0, memory, bytes.Length);

                // Set the last byte in memory to 0 (null terminating character)
                *((byte*)memory.ToPointer() + bytes.Length) = 0;

                return (sbyte*)memory.ToPointer();
            }
        }
    }
}
