using System;
using System.Collections.Generic;

namespace Macaron.Csv.Internal.Headers
{
    partial class EnumHeader<TEnum> where TEnum : struct, IConvertible
    {
        private interface IConverter
        {
            int ToInt32(TEnum value);
        }

        private class RawConverter : IConverter
        {
            private readonly int _maxValue;

            public RawConverter(int maxValue)
            {
                _maxValue = maxValue;
            }

            public int ToInt32(TEnum value)
            {
                var intValue = value.ToInt32(null);
                return intValue < 0 || intValue > _maxValue ? -1 : intValue;
            }
        }

        private class ArrayConverter : IConverter
        {
            private readonly int[] _valueToIndex;

            public ArrayConverter(int[] valueToIndex)
            {
                _valueToIndex = valueToIndex;
            }

            public int ToInt32(TEnum value)
            {
                var intValue = value.ToInt32(null);
                return intValue < 0 || intValue >= _valueToIndex.Length ? -1 : _valueToIndex[intValue];
            }
        }

        private class DictConverter : IConverter
        {
            private readonly Dictionary<TEnum, int> _dict;

            public DictConverter(string[] names, IList<string> columnNames)
            {
                var length = names.Length;
                var dict = new Dictionary<TEnum, int>(length);

                for (int i = 0; i < length; ++i)
                {
                    var name = names[i];
                    var key = (TEnum)Enum.Parse(typeof(TEnum), name);
                    var value = columnNames.IndexOf(name);

                    dict.Add(key, value);
                }

                _dict = dict;
            }

            public int ToInt32(TEnum value)
            {
                int index;
                return _dict.TryGetValue(value, out index) ? index : -1;
            }
        }
    }
}
