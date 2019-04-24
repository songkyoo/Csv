using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Assembly-CSharp-Editor")]
#endif
namespace Macaron.Csv.Internal.Headers
{
    internal partial class EnumHeader<TEnum> : ICsvHeader<TEnum> where TEnum : struct, IConvertible
    {
        #region Static
        public static readonly string TypeErrorMessage;

        static EnumHeader()
        {
            var type = typeof(TEnum);

            if (!type.IsEnum)
            {
                TypeErrorMessage = "열거형 형식이어야 합니다.";
                return;
            }

            // 기본 형식 검사.
            var underlyingType = Enum.GetUnderlyingType(type);
            if (underlyingType != typeof(byte) &&
                underlyingType != typeof(sbyte) &&
                underlyingType != typeof(short) &&
                underlyingType != typeof(ushort) &&
                underlyingType != typeof(int))
            {
                TypeErrorMessage = "기본 형식은 byte, sbyte, short, ushort, int 중 하나여야 합니다.";
                return;
            }

            // 값 중복 확인.
            var values = Enum.GetValues(type);
            var uniqueValues = new HashSet<int>();

            for (int i = 0; i < values.Length; ++i)
            {
                if (!uniqueValues.Add(Convert.ToInt32(values.GetValue(i))))
                {
                    TypeErrorMessage = "중복된 값을 가질 수 없습니다.";
                    return;
                }
            }

            TypeErrorMessage = string.Empty;
        }

        private static bool IsZeroBasedContinuous(int[] values)
        {
            for (int i = 0; i < values.Length; ++i)
            {
                if (values[i] != i)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Fields
        private string[] _columnNames;
        private IConverter _converter;
        #endregion

        #region Constructors
        public EnumHeader(IList<string> columnNames)
        {
            if (TypeErrorMessage.Length > 0)
            {
                throw new ArgumentException(TypeErrorMessage, "TEnum");
            }

            var type = typeof(TEnum);
            var names = Enum.GetNames(type);
            var length = names.Length;

            if (columnNames == null)
            {
                columnNames = names;
            }
            else
            {
                // null 요소와 중복 여부를 검사.
                var uniqueColumnNames = new HashSet<string>();

                for (int i = 0; i < columnNames.Count; ++i)
                {
                    var columnName = columnNames[i];

                    if (columnName == null)
                    {
                        throw new ArgumentException("null 요소를 가질 수 없습니다.", "columnNames");
                    }

                    if (!uniqueColumnNames.Add(columnName))
                    {
                        throw new ArgumentException("중복된 이름을 가질 수 없습니다.", "columnNames");
                    }
                }

                // columnNames가 열거형의 모든 값의 이름을 가지고 있는지 검사.
                for (int i = 0; i < length; ++i)
                {
                    var name = names[i];
                    if (!uniqueColumnNames.Contains(name))
                    {
                        throw new ArgumentException(type.Name + "." + name + "을 찾을 수 없습니다.", "columnNames");
                    }
                }
            }

            // 열거형을 인덱스로 변환하는 IConverter 개체 생성.
            var values = Enum.GetValues(type);
            var intValues = new int[length];

            for (int i = 0; i < length; ++i)
            {
                intValues[i] = Convert.ToInt32(values.GetValue(i));
            }

            var converter = default(IConverter);

            if (IsZeroBasedContinuous(intValues))
            {
                var valueToIndex = new int[length];

                for (int i = 0; i < length; ++i)
                {
                    valueToIndex[i] = columnNames.IndexOf(names[i]);
                }

                if (IsZeroBasedContinuous(valueToIndex))
                {
                    // 열거형이 0에서 시작하고 모든 값이 연속적이며 columnNames와 순서가 일치하는 경우.
                    converter = new RawConverter(length - 1);
                }
                else
                {
                    // 열거형이 0에서 시작하고 모든 값이 연속적이지만 columnNames와 순서가 일치하지 않는 경우.
                    converter = new ArrayConverter(valueToIndex);
                }
            }
            else
            {
                // 열거형이 0에서 시작하지 않거나 연속적이지 않은 경우.
                converter = new DictConverter(names, columnNames);
            }

            _converter = converter;
            _columnNames = new string[columnNames.Count];
            columnNames.CopyTo(_columnNames, 0);
        }
        #endregion

        #region Implementation of ICsvHeader<TEnum>
        public string this[int index]
        {
            get { return _columnNames[index]; }
            set { throw new NotSupportedException(); }
        }

        public int Count
        {
            get { return _columnNames.Length; }
        }

        public bool Contains(string item)
        {
            return (_columnNames as ICollection<string>).Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            _columnNames.CopyTo(array, arrayIndex);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return (_columnNames as IEnumerable<string>).GetEnumerator();
        }

        public int GetIndex(TEnum columnName)
        {
            return _converter.ToInt32(columnName);
        }

        public int IndexOf(string item)
        {
            return Array.IndexOf(_columnNames, item);
        }

        bool ICollection<string>.IsReadOnly
        {
            get { return true; }
        }

        void ICollection<string>.Add(string item)
        {
            throw new NotSupportedException();
        }

        void ICollection<string>.Clear()
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _columnNames.GetEnumerator();
        }

        void IList<string>.Insert(int index, string item)
        {
            throw new NotSupportedException();
        }

        bool ICollection<string>.Remove(string item)
        {
            throw new NotSupportedException();
        }

        void IList<string>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}
