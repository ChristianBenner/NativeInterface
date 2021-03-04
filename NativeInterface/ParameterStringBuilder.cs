using System;

namespace NativeInterface
{
    public class ParameterStringBuilder
    {
        public class ParameterCodes
        {
            public const string VOID = "V";
            public const string BOOLEAN = "Z";
            public const string BYTE = "B";
            public const string CHAR = "C";
            public const string SHORT = "S";
            public const string INT = "I";
            public const string LONG = "J";
            public const string FLOAT = "F";
            public const string DOUBLE = "D";
            public const string STRING = "Ljava/lang/String;";
        }

        private string parameterString;
        private string returnString;

        public ParameterStringBuilder()
        {
            parameterString = "";
            returnString = ParameterCodes.VOID;
        }

        override
        public string ToString()
        {
            return "(" + parameterString + ")" + returnString;
        }

        public ParameterStringBuilder SetReturnType(string parameterCode, bool isArray)
        {
            if (isArray)
            {
                returnString = "[" + parameterCode;
            }
            else
            {
                returnString = parameterCode;
            }

            return this;
        }

        public ParameterStringBuilder SetReturnType(string parameterCode)
        {
            return SetReturnType(parameterCode, false);
        }

        public ParameterStringBuilder AddBoolean()
        {
            parameterString += ParameterCodes.BOOLEAN;
            return this;
        }

        public ParameterStringBuilder AddByte()
        {
            parameterString += ParameterCodes.BYTE;
            return this;
        }

        public ParameterStringBuilder AddChar()
        {
            parameterString += ParameterCodes.CHAR;
            return this;
        }

        public ParameterStringBuilder AddShort()
        {
            parameterString += ParameterCodes.SHORT;
            return this;
        }

        public ParameterStringBuilder AddInt()
        {
            parameterString += ParameterCodes.INT;
            return this;
        }

        public ParameterStringBuilder AddLong()
        {
            parameterString += ParameterCodes.LONG;
            return this;
        }

        public ParameterStringBuilder AddFloat()
        {
            parameterString += ParameterCodes.FLOAT;
            return this;
        }

        public ParameterStringBuilder AddDouble()
        {
            parameterString += ParameterCodes.DOUBLE;
            return this;
        }
        
        public ParameterStringBuilder AddString()
        {
            parameterString += ParameterCodes.STRING;
            return this;
        }

        // Fully qualified class e.g. 'java/lang/String'
        public ParameterStringBuilder AddObject(string fullyQualifiedClass)
        {
            parameterString += ("L" + fullyQualifiedClass + ";");
            return this;
        }
    }
}
