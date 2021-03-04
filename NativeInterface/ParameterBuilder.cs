using System;
using System.Collections.Generic;
using System.Text;

namespace NativeInterface
{
    public unsafe class ParameterBuilder
    {
        private ParameterStringBuilder parameterStringBuilder;
        private List<long> list;

        public ParameterBuilder()
        {
            parameterStringBuilder = new ParameterStringBuilder();
            list = new List<long>();
        }

        public long* Build()
        {
            long[] argBuf = new long[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                argBuf[i] = list[i];
            }

            fixed (long* argumentPointer = argBuf)
            {
                return argumentPointer;
            }
        }

        public string GetParameterString()
        {
            return parameterStringBuilder.ToString();
        }


        public ParameterBuilder SetReturnType(string parameterCode, bool isArray)
        {
            parameterStringBuilder.SetReturnType(parameterCode, isArray);
            return this;
        }

        public ParameterBuilder SetReturnType(string parameterCode)
        {
            parameterStringBuilder.SetReturnType(parameterCode, false);
            return this;
        }

        public ParameterBuilder AddBoolean(bool value)
        {
            list.Add(*(long*)&value);
            parameterStringBuilder.AddBoolean();
            return this;
        }

        public ParameterBuilder AddByte(byte value)
        {
            list.Add(*(long*)&value);
            parameterStringBuilder.AddByte();
            return this;
        }

        public ParameterBuilder AddChar(char value)
        {
            list.Add(*(long*)&value);
            parameterStringBuilder.AddChar();
            return this;
        }

        public ParameterBuilder AddShort(short value)
        {
            list.Add(*(long*)&value);
            parameterStringBuilder.AddShort();
            return this;
        }

        public ParameterBuilder AddInt(int value)
        {
            long* argument = stackalloc long[1];
            argument[0] = *(long*)&value;
            list.Add(*argument);
            parameterStringBuilder.AddInt();
            return this;
        }

        public ParameterBuilder AddLong(long value)
        {
            list.Add(*&value);
            parameterStringBuilder.AddLong();
            return this;
        }

        public ParameterBuilder AddFloat(float value)
        {
            list.Add(*(long*)&value);
            parameterStringBuilder.AddFloat();
            return this;
        }

        public ParameterBuilder AddDouble(double value)
        {
            list.Add(*(long*)&value);
            parameterStringBuilder.AddDouble();
            return this;
        }
        
        public ParameterBuilder AddString(Jvm jvm, string value)
        {
            return AddString(jvm.NewStringUtf(value));
        }

        // Create the string IntPtr using JVM function NewStringUtf
        public ParameterBuilder AddString(IntPtr value)
        {
            list.Add(value.ToInt64());
            parameterStringBuilder.AddString();
            return this;
        }

        // Fully qualified class e.g. 'java/lang/String'
        public ParameterBuilder AddObject(string fullyQualifiedClass, IntPtr objPtr)
        {
            list.Add(objPtr.ToInt64());
            parameterStringBuilder.AddObject(fullyQualifiedClass);
            return this;
        }
    }
}
