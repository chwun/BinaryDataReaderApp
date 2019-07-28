using System;

namespace BinaryDataReaderApp.Models
{
    public static class BinaryValueTypeHelper
    {
        public static int GetLength(BinaryValueType valueType)
        {
            switch (valueType)
            {
                case BinaryValueType.BYTE:
                case BinaryValueType.BOOL:
                    return 1;

                case BinaryValueType.SHORT:
                case BinaryValueType.USHORT:
                    return 2;

                case BinaryValueType.INT:
                case BinaryValueType.UINT:
                case BinaryValueType.FLOAT:
                    return 4;

                case BinaryValueType.LONG:
                case BinaryValueType.ULONG:
                case BinaryValueType.DOUBLE:
                    return 8;

                default:
                    throw new ArgumentException("invalid value type");
            }
        }
    }
}