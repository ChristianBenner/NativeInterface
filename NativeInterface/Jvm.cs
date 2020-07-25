using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace NativeInterface
{
    public unsafe class Jvm
    {
        // JNI version 1.1
        public const int JNI_VERSION_1_1 = 0x00010001;

        // JNI version 1.2
        public const int JNI_VERSION_1_2 = 0x00010002;

        // JNI version 1.4
        public const int JNI_VERSION_1_4 = 0x00010004;

        // JNI version 1.6
        public const int JNI_VERSION_1_6 = 0x00010006;

        // JNI version 1.8
        public const int JNI_VERSION_1_8 = 0x00010008;

        // JNI version 9
        public const int JNI_VERSION_9 = 0x00090000;

        // JNI version 10
        public const int JNI_VERSION_10 = 0x000a0000;

        // Success
        private const int JNI_OK = 0;

        // Unknown error
        private const int JNI_ERR = -1;

        // Thread detached from the VM
        private const int JNI_EDETACHED = -2;

        // JNI version error
        private const int JNI_EVERSION = -3;

        // Not enough memory
        private const int JNI_ENOMEM = -4;

        // VM already created
        private const int JNI_EEXIST = -5;

        // Invalid arguments
        private const int JNI_EINVAL = -6;
        
        // What the expected class path option should looks like
        private const string EXPECTED_CLASS_PATH_OPTION_STRING = "-Djava.class.path=";

        // An example of the class path option usage
        private const string EXAMPLE_CLASS_PATH_OPTION_STRING = EXPECTED_CLASS_PATH_OPTION_STRING + "ApplicationName.jar";

        // Pointer to JVM
        private IntPtr jvm;

        // Pointer to JNI interface
        private IntPtr jniInterface;

        // Java Native Interface invoke methods
        private JniInvokeMethods invokeMethods;
        
        // Java Native Interface native methods
        private JniNativeMethods nativeMethods;

        // Store global references to classes so they do not have to be found multiple times
        private Dictionary<string, IntPtr> classes = new Dictionary<string, IntPtr>();


        public Jvm(string jvmPath, int jniVersion, List<string> options)
        {
            // Check if jvm path was provided
            if(jvmPath == null)
            {
                // Throw an exception
                throw new ArgumentException("Error: Cannot create Java Virtual Machine because no JVM path has been provided");
            }

            // If no options where provided, throw no arguments error. Otherwise throw error including arguments
            if (options == null || options.Count == 0)
            {
                // Throw an exception
                throw new ArgumentException("Error: Cannot create Java Virtual Machine because no arguments have been provided");
            }
            
            checkClassPathProvided(options);
            loadJvmDll(jvmPath);
            createJavaVM(jniVersion, options);
            attachThreadToJVM();
        }

        ~Jvm()
        {
            // Detach thread
            invokeMethods.detachCurrentThread(jvm);

            // Destroy JVM
            invokeMethods.destroyJavaVM(jvm);

            // Destroy Java Virtual Machine
            JvmMethods.JNI_DestroyJavaVM();
        }

        public string JavaStringPtrToUtf8String(IntPtr stringPtr)
        {
            // Check if the string pointer is valid
            if (stringPtr == IntPtr.Zero)
            {
                throw new ArgumentException("Error: Cannot convert null string pointer");
            }
            
            int length = nativeMethods.getStringUTFLength(jniInterface, stringPtr);

            // Unused (stores bool state as to if a copy was made)
            byte isCopyBool;
            
            IntPtr utfStringPtr = nativeMethods.getStringUTFChars(jniInterface, stringPtr, &isCopyBool);
            
            // Conver UTF string to signed bytes
            IntPtr ptr = new IntPtr((sbyte*)utfStringPtr);

            string utf8String = "NULL";
            if (ptr != IntPtr.Zero)
            {
                byte[] bytes = new byte[length];
                Marshal.Copy(ptr, bytes, 0, bytes.Length);
                utf8String = Encoding.UTF8.GetString(bytes);
            }

            return utf8String;
        }

        // Load the Java Virtual Machine DLL
        private void loadJvmDll(string jvmPath)
        {
            // Check if loading the DLL has worked
            if (WindowsMethods.LoadLibrary(jvmPath) == IntPtr.Zero)
            {
                // Get error code from Marshal
                int errorCode = Marshal.GetLastWin32Error();

                // Retrieve the exception
                Win32Exception exception = new Win32Exception(errorCode);

                // Add the last error
                exception.Data.Add("LastWin32Error", errorCode);

                // Throw an exception
                throw new Exception("Error: Failed to load Java Virtual Machine library: '" + jvmPath +
                    "'. Make sure you are running the application in the same directory as the 'dist' folder",
                    exception);
            }
        }

        private void createJavaVM(int jniVersion, List<string> options)
        {
            // Create list of options to provide the Java Virtual Machine
            JavaVMOption[] javaVMOptions = new JavaVMOption[options.Count];

            // Create pointer to option strings
            for (int i = 0; i < options.Count; i++)
            {
                javaVMOptions[i].optionString = Marshal.StringToHGlobalAnsi(options[i]);
            }

            // Create a pointer to the JavaVM options, add it to the JavaVM initialisation arguments
            // and use it to create the JavaVM
            fixed (JavaVMOption* javaVMOptionsPointer = &javaVMOptions[0])
            {
                JavaVMInitArgs javaVMInitArgs = new JavaVMInitArgs();
                javaVMInitArgs.version = jniVersion;
                javaVMInitArgs.nOptions = options.Count;
                javaVMInitArgs.options = javaVMOptionsPointer;

                // Create Java VM
                JvmMethods.JNI_CreateJavaVM(out jvm, out jniInterface, &javaVMInitArgs);
            }

            // Free memory from string pointers
            for (int i = 0; i < javaVMOptions.Length; i++)
            {
                Marshal.FreeHGlobal(javaVMOptions[i].optionString);
            }

            // Check for errors
            if (jvm == IntPtr.Zero)
            {
                // Create a string listing the arguments
                string argumentsString = "";

                // Add each argument
                foreach (string arg in options)
                {
                    argumentsString += arg + "\n";
                }

                // Throw an exception
                throw new Exception("Error: Failed to create Java Virtual Machine using args:\n" + argumentsString);
            }

            // Retrieve the Jni Invoke Methods
            invokeMethods = new JniInvokeMethods(jvm);
        }

        private void checkForJNIException()
        {
            // Exception check returns a boolean represented as a byte
            byte boolResult = nativeMethods.exceptionCheck(jniInterface);

            // If true, an exception has occured
            if (boolResult != 0)
            {
                // Exception has occured
                IntPtr error = nativeMethods.exceptionOccurred(jniInterface);
                
                // Check if an error has occurred
                if (error == IntPtr.Zero)
                {
                    Console.WriteLine("[ERROR] A JVM Exception has occured: ");
                    nativeMethods.exceptionDescribe(jniInterface);

                    throw new Exception("Error: JVM Exception");
                }

                // Clear the exception
                nativeMethods.exceptionClear(jniInterface);
            }
        }

        private void checkClassPathProvided(List<string> options)
        {
            // Keep track of if a class path option has been found in list of options
            bool foundClassPathOption = false;

            // Check if the arguments provided include a class path '-Djava.class.path=Application.jar'
            foreach (string option in options)
            {
                // Check if the option is a class path type
                if (option.StartsWith(EXPECTED_CLASS_PATH_OPTION_STRING))
                {
                    foundClassPathOption = true;
                }
            }

            // If no class path option has been found, throw an error
            if (foundClassPathOption == false)
            {
                // Throw an exception because class path has not been provided
                throw new ArgumentException("Error: Cannot create Java Virtual Machine because no class path has been provided. Add '" +
                    EXAMPLE_CLASS_PATH_OPTION_STRING + "' to list of options");
            }
        }

        private void attachThreadToJVM()
        {
            // Attach thread to VM and obtain JNI interface pointer
            int result = invokeMethods.attachCurrentThread(jvm, out jniInterface, IntPtr.Zero);

            // Check if attached succcesfully
            if (result != JNI_OK)
            {
                throw new Exception("Error: Failed to attach thread to JVM");
            }

            // Retrieve the JNI Native Methods
            nativeMethods = new JniNativeMethods(jniInterface);
        }

        // Uses map so that the class only has to be fetched once
        public IntPtr FindClass(string clazz)
        {
            // Pointer to class
            IntPtr classGlobalRef;

            // Try get value from dictionary, but if its not there then find class and create
            // global ref (and then add the global ref to dictionary)
            if (!classes.TryGetValue(clazz, out classGlobalRef))
            {
                // Find the class
                IntPtr mainClass = nativeMethods.findClass(jniInterface, clazz);

                // Throw error if class not found
                if (mainClass == IntPtr.Zero)
                {
                    throw new Exception("Error: Could not find class '" + clazz + "'");
                }

                // Create global reference to class
                classGlobalRef = nativeMethods.newGlobalRef(jniInterface, mainClass);

                // Global ref could not be created, throw error
                if (classGlobalRef == IntPtr.Zero)
                {
                    throw new Exception("Error: Could not create a global reference to class '" + clazz + "'");
                }

                // Delete local reference
                nativeMethods.deleteLocalRef(jniInterface, mainClass);

                // Add the class global ref to the dictionary
                classes.Add(clazz, classGlobalRef);
            }

            return classGlobalRef;
        }

        public JavaMethod GetStaticMethod(string clazz, string method, string arguments)
        {
            // Load the class
            IntPtr classGlobalRef = FindClass(clazz);

            // Get method from class global reference
            IntPtr methodPtr = nativeMethods.getStaticMethodId(jniInterface, classGlobalRef, method, arguments);

            // Method could not be found, throw error
            if (methodPtr == IntPtr.Zero)
            {
                throw new Exception("Error: Could not find static method '" + method + "' with args '" + arguments +
                    "' in class '" + clazz + "'");
            }

            return new JavaMethod
            {
                clazz = classGlobalRef,
                method = methodPtr
            };
        }

        public IntPtr CreateObject(string clazz)
        {
            return CreateObject(clazz, "()V", null);
        }

        public IntPtr CreateObject(string clazz, string constructorArgumentFormat, long* constructorArguments)
        {
            // Find the constructor in the sensor data class
            JavaMethod methodInfo = GetMethod(clazz, "<init>", constructorArgumentFormat);

            // Call the constructor
            IntPtr objectRef = nativeMethods.newObject(jniInterface, methodInfo.clazz, methodInfo.method, constructorArguments);
            
            // Global ref could not be created, throw error
            if (objectRef == IntPtr.Zero)
            {
                throw new Exception("Error: Could not create object of type '" + clazz + "'");
            }

            // Object global ref
            IntPtr objectGlobalRef = nativeMethods.newGlobalRef(jniInterface, objectRef);

            // Global ref could not be created, throw error
            if (objectGlobalRef == IntPtr.Zero)
            {
                throw new Exception("Error: Could not create a global reference object of type '" + clazz + "'");
            }

            // Delete local reference
            nativeMethods.deleteLocalRef(jniInterface, objectRef);

            return objectGlobalRef;
        }

        public JavaMethod GetMethod(string clazz, string method, string arguments)
        {
            // Load the class
            IntPtr classGlobalRef = FindClass(clazz);

            // Get method from class global reference
            IntPtr methodPtr = nativeMethods.getMethodID(jniInterface, classGlobalRef, method, arguments);

            // Method could not be found, throw error
            if (methodPtr == IntPtr.Zero)
            {
                throw new Exception("Error: Could not find method '" + method + "' with args '" + arguments +
                    "' in class '" + clazz + "'");
            }

            return new JavaMethod
            {
                clazz = classGlobalRef,
                method = methodPtr
            };
        }

        public void CallStaticVoidMethod(JavaMethod methodInfo)
        {
            CallStaticVoidMethod(methodInfo, null);
        }

        public void CallStaticVoidMethod(JavaMethod methodInfo, long* args)
        {
            nativeMethods.callStaticVoidMethod(jniInterface, methodInfo.clazz, methodInfo.method, args);
            checkForJNIException();
        }

        public void CallVoidMethod(IntPtr objectRef, JavaMethod methodInfo)
        {
            CallVoidMethod(objectRef, methodInfo, null);
        }

        public void CallVoidMethod(IntPtr objectRef, JavaMethod methodInfo, long* args)
        {
            nativeMethods.callVoidMethod(jniInterface, objectRef, methodInfo.method, args);
            checkForJNIException();
        }

        public void RegisterNatives(string clazz, NativeJavaMethod[] methods)
        {
            // Get global reference to class
            IntPtr globalRef = FindClass(clazz);
            
            // Fixed required creating pointer to array
            fixed (NativeJavaMethod* method = &methods[0])
            {
                // Register the methods
                int result = nativeMethods.registerNatives(jniInterface, globalRef, method, methods.Length);

                // Check if an error occurred
                if (result != JNI_OK)
                {
                    throw new Exception("Error: Failed to register native methods with class '" + clazz + "'");
                }
            }

            // After using fixed, we now have to free the memory for each method
            foreach (NativeJavaMethod method in methods)
            {
                Marshal.FreeHGlobal(method.name);
                Marshal.FreeHGlobal(method.signature);
            }

            // Remove class from dictionary
            classes.Remove(clazz);

            // Remove global reference
            nativeMethods.deleteGlobalRef(jniInterface, globalRef);
        }
    }
}
