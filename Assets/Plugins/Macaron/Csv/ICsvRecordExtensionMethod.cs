using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Macaron.Csv
{
    public static class ICsvRecordExtensionMethod
    {
        /// <summary>
        /// 필드와 관련된 정보를 가지는 구조체.
        /// </summary>
        public struct Field : IEquatable<Field>
        {
            /// <summary>
            /// 필드가 속한 레코드 번호.
            /// </summary>
            public readonly int RecordNumber;

            /// <summary>
            /// 필드의 열 이름.
            /// </summary>
            public readonly string ColumnName;

            /// <summary>
            /// 필드의 값.
            /// </summary>
            public readonly string Value;

            public Field(int recordNumber, string columnName, string value)
            {
                RecordNumber = recordNumber;
                ColumnName = columnName;
                Value = value;
            }

            #region Operators
            public static bool operator ==(Field lhs, Field rhs)
            {
                return lhs.Equals(rhs);
            }

            public static bool operator !=(Field lhs, Field rhs)
            {
                return !(lhs == rhs);
            }
            #endregion

            #region Overrides
            public override bool Equals(object obj)
            {
                if (obj is Field)
                {
                    return Equals((Field)obj);
                }

                return false;
            }

            public override int GetHashCode()
            {
                var h1 = RecordNumber;
                var h2 = ColumnName != null ? ColumnName.GetHashCode() : 0;
                var h3 = Value != null ? Value.GetHashCode() : 0;

                unchecked
                {
                    var result = 0;
                    result = (result * 397) ^ h1;
                    result = (result * 397) ^ h2;
                    result = (result * 397) ^ h3;

                    return result;
                }
            }
            #endregion

            #region Implementation of IEquatable<Field>
            public bool Equals(Field other)
            {
                return RecordNumber == other.RecordNumber && ColumnName == other.ColumnName && Value == other.Value;
            }
            #endregion
        }

        private static readonly Field[] EmptyFields = new Field[0];

        /// <summary>
        /// 지정한 열 이름의 필드값을 이용해 <see cref="Field"/> 개체를 생성한다.
        /// </summary>
        /// <param name="columnName">열 이름.</param>
        /// <typeparam name="T">열 이름 형식.</typeparam>
        /// <returns>지정한 열 이름의 필드와 관련된 정보를 가지는 <see cref="Field"/> 개체.</returns>
        /// <exception cref="ArgumentException">레코드에서 <paramref name="columnName"/>으로 필드를 찾을 수 없는 경우.</exception>
        public static Field Parse<T>(this ICsvRecord<T> record, T columnName)
        {
            var header = record.Header;
            int index = header.GetIndex(columnName);

            if (index == -1)
            {
                throw new ArgumentException(columnName + "은 유효한 열 이름이 아닙니다.", "columnName");
            }

            return new Field(record.RecordNumber, header[index], record[index]);
        }

        /// <summary>
        /// <see cref="Field.Value"/>값을 <paramref name="pattern"/>으로 분할하여 새로운 <see cref="Field"/> 배열을 반환한다.
        /// </summary>
        /// <param name="pattern">분할 기준이되는 정규표현식.</param>
        /// <returns><paramref name="pattern"/>으로 분할된 <see cref="Field.Value"/>값을 가지는 <see cref="Field"/> 배열.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="pattern"/>이 <c>null</c>인 경우.</exception>
        public static Field[] Split(this Field field, Regex pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }

            var value = field.Value;

            if (string.IsNullOrEmpty(value))
            {
                return EmptyFields;
            }

            var values = pattern.Split(value);
            var results = new Field[values.Length];

            for (int i = 0; i < values.Length; ++i)
            {
                results[i] = new Field(field.RecordNumber, field.ColumnName, values[i]);
            }

            return results;
        }

        /// <summary>
        /// <see cref="Field.Value"/>값을 <paramref name="separator"/>로 분할하여 새로운 <see cref="Field"/> 배열을 반환한다.
        /// </summary>
        /// <param name="separator">분할 기준이되는 문자열.</param>
        /// <returns><paramref name="separator"/>로 분할된 <see cref="Field.Value"/>값을 가지는 <see cref="Field"/> 배열.</returns>
        public static Field[] Split(this Field field, string separator)
        {
            var value = field.Value;

            if (string.IsNullOrEmpty(value))
            {
                return EmptyFields;
            }

            if (string.IsNullOrEmpty(separator))
            {
                return new[] { field };
            }

            var startIndex = 0;
            var endIndex = value.IndexOf(separator[0]);
            var lastIndex = value.Length - separator.Length;

            if (endIndex == -1 || endIndex > lastIndex)
            {
                return new[] { field };
            }

            var results = new List<Field>();

            while (endIndex != -1 && endIndex <= lastIndex)
            {
                if (string.CompareOrdinal(value, endIndex, separator, 0, separator.Length) == 0)
                {
                    results.Add(new Field(
                        field.RecordNumber,
                        field.ColumnName,
                        value.Substring(startIndex, endIndex - startIndex)));
                    endIndex += separator.Length;
                    startIndex = endIndex;
                }

                endIndex = value.IndexOf(separator[0], endIndex);
            }

            results.Add(new Field(field.RecordNumber, field.ColumnName, value.Substring(startIndex)));

            return results.ToArray();
        }

        /// <summary>
        /// <see cref="Field.Value"/>값을 <paramref name="separator"/>로 분할하여 새로운 <see cref="Field"/> 배열을 반환한다.
        /// </summary>
        /// <param name="separator">분할 기준이되는 문자.</param>
        /// <returns><paramref name="separator"/>로 분할된 <see cref="Field.Value"/>값을 가지는 <see cref="Field"/> 배열.</returns>
        public static Field[] Split(this Field field, char separator)
        {
            var value = field.Value;

            if (string.IsNullOrEmpty(value))
            {
                return EmptyFields;
            }

            var startIndex = 0;
            var endIndex = value.IndexOf(separator);

            if (endIndex == -1)
            {
                return new[] { field };
            }

            var results = new List<Field>();

            while (endIndex != -1)
            {
                results.Add(new Field(
                    field.RecordNumber,
                    field.ColumnName,
                    value.Substring(startIndex, endIndex - startIndex)));
                startIndex = endIndex + 1;
                endIndex = value.IndexOf(separator, startIndex);
            }

            results.Add(new Field(field.RecordNumber, field.ColumnName, value.Substring(startIndex)));

            return results.ToArray();
        }

        /// <summary>
        /// 필드값을 <see cref="Boolean?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Boolean?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static bool? AsBoolean(this Field field)
        {
            var converter = new NullableBooleanConverter();
            return As<bool?, NullableBooleanConverter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="Byte?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Byte?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static byte? AsByte(
            this Field field,
            NumberStyles styles = NumberStyles.Integer,
            IFormatProvider provider = null)
        {
            var converter = new NullableByteConverter(styles, provider);
            return As<byte?, NullableByteConverter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="Char"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Char"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static char? AsChar(this Field field)
        {
            var converter = new NullableCharConverter();
            return As<char?, NullableCharConverter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="Decimal?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Decimal?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static decimal? AsDecimal(
            this Field field,
            NumberStyles styles = NumberStyles.Number,
            IFormatProvider provider = null)
        {
            var converter = new NullableDecimalConverter(styles, provider);
            return As<decimal?, NullableDecimalConverter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="Double?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Double?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static double? AsDouble(
            this Field field,
            NumberStyles styles = NumberStyles.Float | NumberStyles.AllowThousands,
            IFormatProvider provider = null)
        {
            var converter = new NullableDoubleConverter(styles, provider);
            return As<double?, NullableDoubleConverter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="Int16?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Int16?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static short? AsInt16(
            this Field field,
            NumberStyles styles = NumberStyles.Integer,
            IFormatProvider provider = null)
        {
            var converter = new NullableInt16Converter(styles, provider);
            return As<short?, NullableInt16Converter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="Int32?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Int32?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static int? AsInt32(
            this Field field,
            NumberStyles styles = NumberStyles.Integer,
            IFormatProvider provider = null)
        {
            var converter = new NullableInt32Converter(styles, provider);
            return As<int?, NullableInt32Converter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="Int64?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Int64?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static long? AsInt64(
            this Field field,
            NumberStyles styles = NumberStyles.Integer,
            IFormatProvider provider = null)
        {
            var converter = new NullableInt64Converter(styles, provider);
            return As<long?, NullableInt64Converter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="SByte?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="SByte?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static sbyte? AsSByte(
            this Field field,
            NumberStyles styles = NumberStyles.Integer,
            IFormatProvider provider = null)
        {
            var converter = new NullableSByteConverter(styles, provider);
            return As<sbyte?, NullableSByteConverter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="Single?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Single?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static float? AsSingle(
            this Field field,
            NumberStyles styles = NumberStyles.Float | NumberStyles.AllowThousands,
            IFormatProvider provider = null)
        {
            var converter = new NullableSingleConverter(styles, provider);
            return As<float?, NullableSingleConverter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="UInt16"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="UInt16"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static ushort? AsUInt16(
            this Field field,
            NumberStyles styles = NumberStyles.Integer,
            IFormatProvider provider = null)
        {
            var converter = new NullableUInt16Converter(styles, provider);
            return As<ushort?, NullableUInt16Converter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="UInt32?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="UInt32?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static uint? AsUInt32(
            this Field field,
            NumberStyles styles = NumberStyles.Integer,
            IFormatProvider provider = null)
        {
            var converter = new NullableUInt32Converter(styles, provider);
            return As<uint?, NullableUInt32Converter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="UInt64?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="UInt64?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static ulong? AsUInt64(
            this Field field,
            NumberStyles styles = NumberStyles.Integer,
            IFormatProvider provider = null)
        {
            var converter = new NullableUInt64Converter(styles, provider);
            return As<ulong?, NullableUInt64Converter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <typeparamref name="TEnum?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="ignoreCase"><c>true</c>면 대소문자를 무시하고 <c>false</c>면 대소문자를 구분한다.</param>
        /// <typeparam name="TEnum">열거형 형식.</typeparam>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <typeparamref name="TEnum?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/>이 열거형 형식이 아닌 경우.</exception>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static TEnum? AsEnum<TEnum>(this Field field, bool ignoreCase = false)
             where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var converter = new NullableEnumConverter<TEnum>(ignoreCase);
            return As<TEnum?, NullableEnumConverter<TEnum>>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="DateTime?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="DateTime?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static DateTime? AsDateTime(
            this Field field,
            IFormatProvider provider = null,
            DateTimeStyles styles = DateTimeStyles.None)
        {
            var converter = new NullableDateTimeConverter(null, null, provider, styles);
            return As<DateTime?, NullableDateTimeConverter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="DateTime?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="format"><c>null</c>이나 빈 문자열이 아닌 경우 지정한 서식의 문자열만 허용된다.</param>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="DateTime?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static DateTime? AsDateTime(
            this Field field,
            string format,
            IFormatProvider provider = null,
            DateTimeStyles styles = DateTimeStyles.None)
        {
            var converter = new NullableDateTimeConverter(format, null, provider, styles);
            return As<DateTime?, NullableDateTimeConverter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="DateTime?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="formats"><c>null</c>이 아닌 경우 지정한 서식 목록의 문자열만 허용된다.</param>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="DateTime?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static DateTime? AsDateTime(
            this Field field,
            string[] formats,
            IFormatProvider provider = null,
            DateTimeStyles styles = DateTimeStyles.None)
        {
            var converter = new NullableDateTimeConverter(null, formats, provider, styles);
            return As<DateTime?, NullableDateTimeConverter>(field, converter);
        }

#if !UNITY_5_6_OR_NEWER || NET_2_0 || NET_2_0_SUBSET
        /// <summary>
        /// 필드값을 <see cref="TimeSpan"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="TimeSpan"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static TimeSpan? AsTimeSpan(this Field field)
        {
            var converter = new NullableTimeSpanConverter();
            return As<TimeSpan?, NullableTimeSpanConverter>(field, converter);
        }
#else

        /// <summary>
        /// 필드값을 <see cref="TimeSpan?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="TimeSpan?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static TimeSpan? AsTimeSpan(
            this Field field,
            IFormatProvider provider = null,
            TimeSpanStyles styles = TimeSpanStyles.None)
        {
            var converter = new NullableTimeSpanConverter(null, null, provider, styles);
            return As<TimeSpan?, NullableTimeSpanConverter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="TimeSpan?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="format"><c>null</c>이나 빈 문자열이 아닌 경우 지정한 서식의 문자열만 허용된다.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="TimeSpan?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static TimeSpan? AsTimeSpan(
            this Field field,
            string format,
            IFormatProvider provider = null,
            TimeSpanStyles styles = TimeSpanStyles.None)
        {
            var converter = new NullableTimeSpanConverter(format, null, provider, styles);
            return As<TimeSpan?, NullableTimeSpanConverter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="TimeSpan?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="formats"><c>null</c>이 아닌 경우 지정한 서식 목록의 문자열만 허용된다.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="TimeSpan?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static TimeSpan? AsTimeSpan(
            this Field field,
            string[] formats,
            IFormatProvider provider = null,
            TimeSpanStyles styles = TimeSpanStyles.None)
        {
            var converter = new NullableTimeSpanConverter(null, formats, provider, styles);
            return As<TimeSpan?, NullableTimeSpanConverter>(field, converter);
        }
#endif

        /// <summary>
        /// 필드값을 <see cref="DateTimeOffset?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="DateTimeOffset?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static DateTimeOffset? AsDateTimeOffset(
            this Field field,
            IFormatProvider provider = null,
            DateTimeStyles styles = DateTimeStyles.None)
        {
            var converter = new NullableDateTimeOffsetConverter(null, null, provider, styles);
            return As<DateTimeOffset?, NullableDateTimeOffsetConverter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="DateTimeOffset?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="format"><c>null</c>이나 빈 문자열이 아닌 경우 지정한 서식의 문자열만 허용된다.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="DateTimeOffset?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static DateTimeOffset? AsDateTimeOffset(
            this Field field,
            string format,
            IFormatProvider provider = null,
            DateTimeStyles styles = DateTimeStyles.None)
        {
            var converter = new NullableDateTimeOffsetConverter(format, null, provider, styles);
            return As<DateTimeOffset?, NullableDateTimeOffsetConverter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="DateTimeOffset?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="formats"><c>null</c>이 아닌 경우 지정한 서식 목록의 문자열만 허용된다.</param>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <param name="styles">필드값이 어떤 형식을 가지고 있는지 나타내는 열거형의 조합.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="DateTimeOffset?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static DateTimeOffset? AsDateTimeOffset(
            this Field field,
            string[] formats,
            IFormatProvider provider = null,
            DateTimeStyles styles = DateTimeStyles.None)
        {
            var converter = new NullableDateTimeOffsetConverter(null, formats, provider, styles);
            return As<DateTimeOffset?, NullableDateTimeOffsetConverter>(field, converter);
        }

#if !UNITY_5_6_OR_NEWER || NET_2_0 || NET_2_0_SUBSET
        /// <summary>
        /// 필드값을 <see cref="Guid?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Guid?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static Guid? AsGuid(this Field field)
        {
            var converter = new NullableGuidConverter();
            return As<Guid?, NullableGuidConverter>(field, converter);
        }
#else
        /// <summary>
        /// 필드값을 <see cref="Guid?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Guid?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static Guid? AsGuid(this Field field)
        {
            var converter = new NullableGuidConverter(null);
            return As<Guid?, NullableGuidConverter>(field, converter);
        }

        /// <summary>
        /// 필드값을 <see cref="Guid?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="format"><c>null</c>이나 빈 문자열이 아닌 경우 지정한 서식의 문자열만 허용된다.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Guid?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static Guid? AsGuid(this Field field, string format)
        {
            var converter = new NullableGuidConverter(format);
            return As<Guid?, NullableGuidConverter>(field, converter);
        }
#endif

        /// <summary>
        /// 필드값이 빈 문자열이거나 <c>null</c>이면 <c>null</c>을 반환하고 그렇지 않다면 그대로 반환한다.
        /// </summary>
        /// <returns>필드값이 빈 문자열이거나 <c>null</c>이면 <c>null</c>, 그렇지 않다면 그대로 반환한다.</returns>
        public static string AsString(this Field field)
        {
            return string.IsNullOrEmpty(field.Value) ? null : field.Value;
        }

        /// <summary>
        /// 필드값을 <see cref="Uri"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="uriKind">문자열이 상대 경로나 절대 경로인지 직접 지정하거나 문자열을 보고 판단하도록 한다.</param>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Uri"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static Uri AsUri(this Field field, UriKind uriKind = UriKind.Absolute)
        {
            var converter = new UriConverter(uriKind);
            return As<Uri, UriConverter>(field, converter);
        }

        private interface IConverter<T>
        {
            T Convert(string value);
        }

        #region Converters
        private struct NullableBooleanConverter : IConverter<bool?>
        {
            public bool? Convert(string value)
            {
                return bool.Parse(value);
            }
        }

        private struct NullableByteConverter : IConverter<byte?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableByteConverter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public byte? Convert(string value)
            {
                return byte.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableCharConverter : IConverter<char?>
        {
            public char? Convert(string value)
            {
                return char.Parse(value);
            }
        }

        private struct NullableDecimalConverter : IConverter<decimal?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableDecimalConverter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public decimal? Convert(string value)
            {
                return decimal.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableDoubleConverter : IConverter<double?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableDoubleConverter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public double? Convert(string value)
            {
                return double.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableInt16Converter : IConverter<short?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableInt16Converter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public short? Convert(string value)
            {
                return short.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableInt32Converter : IConverter<int?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableInt32Converter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public int? Convert(string value)
            {
                return int.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableInt64Converter : IConverter<long?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableInt64Converter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public long? Convert(string value)
            {
                return long.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableSByteConverter : IConverter<sbyte?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableSByteConverter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public sbyte? Convert(string value)
            {
                return sbyte.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableSingleConverter : IConverter<float?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableSingleConverter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public float? Convert(string value)
            {
                return float.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableUInt16Converter : IConverter<ushort?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableUInt16Converter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public ushort? Convert(string value)
            {
                return ushort.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableUInt32Converter : IConverter<uint?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableUInt32Converter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public uint? Convert(string value)
            {
                return uint.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableUInt64Converter : IConverter<ulong?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableUInt64Converter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public ulong? Convert(string value)
            {
                return ulong.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableEnumConverter<T> : IConverter<T?> where T : struct
        {
            private bool _ignoreCase;

            public NullableEnumConverter(bool ignoreCase)
            {
                _ignoreCase = ignoreCase;
            }

            public T? Convert(string value)
            {
                return (T)Enum.Parse(typeof(T), value, _ignoreCase);
            }
        }

        private struct NullableDateTimeConverter : IConverter<DateTime?>
        {
            private string _format;
            private string[] _formats;
            private IFormatProvider _provider;
            private DateTimeStyles _styles;

            public NullableDateTimeConverter(
                string format,
                string[] formats,
                IFormatProvider provider,
                DateTimeStyles styles)
            {
                _format = format;
                _formats = formats;
                _provider = provider;
                _styles = styles;
            }

            public DateTime? Convert(string value)
            {
                var provider = _provider ?? DateTimeFormatInfo.CurrentInfo;

                if (!string.IsNullOrEmpty(_format))
                {
                    return DateTime.ParseExact(value, _format, provider, _styles);
                }
                else if (_formats != null)
                {
                    return DateTime.ParseExact(value, _formats, provider, _styles);
                }
                else
                {
                    return DateTime.Parse(value, provider, _styles);
                }
            }
        }

#if !UNITY_5_6_OR_NEWER || NET_2_0 || NET_2_0_SUBSET
        private struct NullableTimeSpanConverter : IConverter<TimeSpan?>
        {
            public TimeSpan? Convert(string value)
            {
                return TimeSpan.Parse(value);
            }
        }
#else
        private struct NullableTimeSpanConverter : IConverter<TimeSpan?>
        {
            private string _format;
            private string[] _formats;
            private IFormatProvider _provider;
            private TimeSpanStyles _styles;

            public NullableTimeSpanConverter(
                string format,
                string[] formats,
                IFormatProvider provider,
                TimeSpanStyles styles)
            {
                _format = format;
                _formats = formats;
                _provider = provider;
                _styles = styles;
            }

            public TimeSpan? Convert(string value)
            {
                var provider = _provider ?? DateTimeFormatInfo.CurrentInfo;

                if (!string.IsNullOrEmpty(_format))
                {
                    return TimeSpan.ParseExact(value, _format, provider, _styles);
                }
                else if (_formats != null)
                {
                    return TimeSpan.ParseExact(value, _formats, provider, _styles);
                }
                else
                {
                    return TimeSpan.Parse(value, provider);
                }
            }
        }
#endif

        private struct NullableDateTimeOffsetConverter : IConverter<DateTimeOffset?>
        {
            private string _format;
            private string[] _formats;
            private IFormatProvider _provider;
            private DateTimeStyles _styles;

            public NullableDateTimeOffsetConverter(
                string format,
                string[] formats,
                IFormatProvider provider,
                DateTimeStyles styles)
            {
                _format = format;
                _formats = formats;
                _provider = provider;
                _styles = styles;
            }

            public DateTimeOffset? Convert(string value)
            {
                var provider = _provider ?? DateTimeFormatInfo.CurrentInfo;

                if (!string.IsNullOrEmpty(_format))
                {
                    return DateTimeOffset.ParseExact(value, _format, provider, _styles);
                }
                else if (_formats != null)
                {
                    return DateTimeOffset.ParseExact(value, _formats, provider, _styles);
                }
                else
                {
                    return DateTimeOffset.Parse(value, provider, _styles);
                }
            }
        }

#if !UNITY_5_6_OR_NEWER || NET_2_0 || NET_2_0_SUBSET
        private struct NullableGuidConverter : IConverter<Guid?>
        {
            public Guid? Convert(string value)
            {
                return new Guid(value);
            }
        }
#else
        private struct NullableGuidConverter : IConverter<Guid?>
        {
            private string _format;

            public NullableGuidConverter(string format)
            {
                _format = format;
            }

            public Guid? Convert(string value)
            {
                if (!string.IsNullOrEmpty(_format))
                {
                    return Guid.ParseExact(value, _format);
                }
                else
                {
                    return Guid.Parse(value);
                }
            }
        }
#endif

        private struct UriConverter : IConverter<Uri>
        {
            private UriKind _uriKind;

            public UriConverter(UriKind uriKind)
            {
                _uriKind = uriKind;
            }

            public Uri Convert(string value)
            {
                return new Uri(value, _uriKind);
            }
        }
        #endregion

        private static TValue As<TValue, TConverter>(Field field, TConverter converter)
            where TConverter : struct, IConverter<TValue>
        {
            if (string.IsNullOrEmpty(field.Value))
            {
                return default(TValue);
            }

            try
            {
                return converter.Convert(field.Value);
            }
            catch (Exception e)
            {
                throw new CsvReaderException(
                    "필드값을 변환할 수 없습니다.",
                    e,
                    field.RecordNumber,
                    field.ColumnName);
            }
        }
    }
}
