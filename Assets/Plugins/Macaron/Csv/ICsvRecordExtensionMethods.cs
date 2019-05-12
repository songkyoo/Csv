using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Macaron.Csv
{
    public static partial class ICsvRecordExtensionMethod
    {
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
            var index = header.GetIndex(columnName);

            if (index == -1)
            {
                throw new ArgumentException("유효한 열 이름이 아닙니다.", "columnName");
            }

            return new Field(record.RecordNumber, header[index], record[index]);
        }

        /// <summary>
        /// 지정한 열 이름 목록의 필드값을 이용해 <see cref="Field"/> 배열을 생성한다.
        /// </summary>
        /// <param name="columnName1">첫 번째 열 이름.</param>
        /// <param name="columnName2">두 번째 열 이름.</param>
        /// <typeparam name="T">열 이름 형식.</typeparam>
        /// <returns>지정한 열 이름 목록의 필드와 관련된 정보를 가지는 <see cref="Field"/> 배열.</returns>
        /// <exception cref="ArgumentException">레코드에서 <paramref name="columnName1"/>으로 필드를 찾을 수 없는 경우.</exception>
        /// <exception cref="ArgumentException">레코드에서 <paramref name="columnName2"/>으로 필드를 찾을 수 없는 경우.</exception>
        public static Field[] Parse<T>(this ICsvRecord<T> record, T columnName1, T columnName2)
        {
            var header = record.Header;
            var index1 = header.GetIndex(columnName1);

            if (index1 == -1)
            {
                throw new ArgumentException("유효한 열 이름이 아닙니다.", "columnName1");
            }

            var index2 = header.GetIndex(columnName2);

            if (index2 == -1)
            {
                throw new ArgumentException("유효한 열 이름이 아닙니다.", "columnName2");
            }

            var field1 = new Field(record.RecordNumber, header[index1], record[index1]);
            var field2 = new Field(record.RecordNumber, header[index2], record[index2]);

            return new[] { field1, field2 };
        }

        /// <summary>
        /// 지정한 열 이름 목록의 필드값을 이용해 <see cref="Field"/> 배열을 생성한다.
        /// </summary>
        /// <param name="columnName1">첫 번째 열 이름.</param>
        /// <param name="columnName2">두 번째 열 이름.</param>
        /// <param name="columnName3">세 번째 열 이름.</param>
        /// <typeparam name="T">열 이름 형식.</typeparam>
        /// <returns>지정한 열 이름 목록의 필드와 관련된 정보를 가지는 <see cref="Field"/> 배열.</returns>
        /// <exception cref="ArgumentException">레코드에서 <paramref name="columnName1"/>으로 필드를 찾을 수 없는 경우.</exception>
        /// <exception cref="ArgumentException">레코드에서 <paramref name="columnName2"/>으로 필드를 찾을 수 없는 경우.</exception>
        /// <exception cref="ArgumentException">레코드에서 <paramref name="columnName3"/>으로 필드를 찾을 수 없는 경우.</exception>
        public static Field[] Parse<T>(this ICsvRecord<T> record, T columnName1, T columnName2, T columnName3)
        {
            var header = record.Header;
            var index1 = header.GetIndex(columnName1);

            if (index1 == -1)
            {
                throw new ArgumentException("유효한 열 이름이 아닙니다.", "columnName1");
            }

            var index2 = header.GetIndex(columnName2);

            if (index2 == -1)
            {
                throw new ArgumentException("유효한 열 이름이 아닙니다.", "columnName2");
            }

            var index3 = header.GetIndex(columnName3);

            if (index3 == -1)
            {
                throw new ArgumentException("유효한 열 이름이 아닙니다.", "columnName3");
            }

            var field1 = new Field(record.RecordNumber, header[index1], record[index1]);
            var field2 = new Field(record.RecordNumber, header[index2], record[index2]);
            var field3 = new Field(record.RecordNumber, header[index3], record[index3]);

            return new[] { field1, field2, field3 };
        }

        /// <summary>
        /// 지정한 열 이름 목록의 필드값을 이용해 <see cref="Field"/> 배열을 생성한다.
        /// </summary>
        /// <param name="columnName1">첫 번째 열 이름.</param>
        /// <param name="columnName2">두 번째 열 이름.</param>
        /// <param name="columnName3">세 번째 열 이름.</param>
        /// <param name="columnName4">네 번째 열 이름.</param>
        /// <typeparam name="T">열 이름 형식.</typeparam>
        /// <returns>지정한 열 이름 목록의 필드와 관련된 정보를 가지는 <see cref="Field"/> 배열.</returns>
        /// <exception cref="ArgumentException">레코드에서 <paramref name="columnName1"/>으로 필드를 찾을 수 없는 경우.</exception>
        /// <exception cref="ArgumentException">레코드에서 <paramref name="columnName2"/>으로 필드를 찾을 수 없는 경우.</exception>
        /// <exception cref="ArgumentException">레코드에서 <paramref name="columnName3"/>으로 필드를 찾을 수 없는 경우.</exception>
        /// <exception cref="ArgumentException">레코드에서 <paramref name="columnName4"/>으로 필드를 찾을 수 없는 경우.</exception>
        public static Field[] Parse<T>(this ICsvRecord<T> record, T columnName1, T columnName2, T columnName3, T columnName4)
        {
            var header = record.Header;
            var index1 = header.GetIndex(columnName1);

            if (index1 == -1)
            {
                throw new ArgumentException("유효한 열 이름이 아닙니다.", "columnName1");
            }

            var index2 = header.GetIndex(columnName2);

            if (index2 == -1)
            {
                throw new ArgumentException("유효한 열 이름이 아닙니다.", "columnName2");
            }

            var index3 = header.GetIndex(columnName3);

            if (index3 == -1)
            {
                throw new ArgumentException("유효한 열 이름이 아닙니다.", "columnName3");
            }

            var index4 = header.GetIndex(columnName4);

            if (index4 == -1)
            {
                throw new ArgumentException("유효한 열 이름이 아닙니다.", "columnName4");
            }

            var field1 = new Field(record.RecordNumber, header[index1], record[index1]);
            var field2 = new Field(record.RecordNumber, header[index2], record[index2]);
            var field3 = new Field(record.RecordNumber, header[index3], record[index3]);
            var field4 = new Field(record.RecordNumber, header[index4], record[index4]);

            return new[] { field1, field2, field3, field4 };
        }

        /// <summary>
        /// 지정한 열 이름 목록의 필드값을 이용해 <see cref="Field"/> 배열을 생성한다.
        /// </summary>
        /// <param name="columnNames">열 이름 목록.</param>
        /// <typeparam name="T">열 이름 형식.</typeparam>
        /// <returns>지정한 열 이름 목록의 필드와 관련된 정보를 가지는 <see cref="Field"/> 배열.</returns>
        /// <exception cref="ArgumentException">레코드에서 <paramref name="columnNames"/>의 요소로 필드를 찾을 수 없는 경우.</exception>
        public static Field[] Parse<T>(this ICsvRecord<T> record, params T[] columnNames)
        {
            var header = record.Header;
            var fields = new Field[columnNames.Length];

            for (int i = 0; i < columnNames.Length; ++i)
            {
                var index = header.GetIndex(columnNames[i]);

                if (index == -1)
                {
                    throw new ArgumentException("유효한 열 이름이 아닌 요소를 포함하고 있습니다.", "columnNames");
                }

                fields[i] = new Field(record.RecordNumber, header[index], record[index]);
            }

            return fields;
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

        /// <summary>
        /// 요소의 수가 두 개인 필드값 목록을 <see cref="Vector2?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값 목록이 비어있거나 모든 필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Vector2?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">요소의 수가 올바르지 않은 경우.</exception>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        /// <remarks>
        /// 필드값 목록의 요소중 일부만 없다면 해당 요소는 0으로 치환된다.
        /// </remarks>
        public static Vector2? AsVector2(this Field[] fields, IFormatProvider provider = null)
        {
            if (fields.Length == 0)
            {
                return null;
            }

            if (fields.Length != 2)
            {
                throw new CsvReaderException(
                    "필드값 목록의 요소 수가 올바르지 않습니다.",
                    null,
                    fields[0].RecordNumber,
                    GetUniqueColumnNames(fields));
            }

            float? x = fields[0].AsSingle(provider: provider);
            float? y = fields[1].AsSingle(provider: provider);

            if (x == null && y == null)
            {
                return null;
            }

            return new Vector2(x ?? 0.0f, y ?? 0.0f);
        }

        /// <summary>
        /// 요소의 수가 세 개인 필드값 목록을 <see cref="Vector3?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값 목록이 비어있거나 모든 필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Vector3?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">요소의 수가 올바르지 않은 경우.</exception>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        /// <remarks>
        /// 필드값 목록의 요소중 일부만 없다면 해당 요소는 0으로 치환된다.
        /// </remarks>
        public static Vector3? AsVector3(this Field[] fields, IFormatProvider provider = null)
        {
            if (fields.Length == 0)
            {
                return null;
            }

            if (fields.Length != 3)
            {
                throw new CsvReaderException(
                    "필드값 목록의 요소 수가 올바르지 않습니다.",
                    null,
                    fields[0].RecordNumber,
                    GetUniqueColumnNames(fields));
            }

            float? x = fields[0].AsSingle(provider: provider);
            float? y = fields[1].AsSingle(provider: provider);
            float? z = fields[2].AsSingle(provider: provider);

            if (x == null && y == null && z == null)
            {
                return null;
            }

            return new Vector3(x ?? 0.0f, y ?? 0.0f, z ?? 0.0f);
        }

        /// <summary>
        /// 16진수 색상 코드값을 <see cref="Color32?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Color32?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static Color32? AsColor32(this Field field)
        {
            if (string.IsNullOrEmpty(field.Value))
            {
                return null;
            }

            var converter = new NullableColor32Converter();
            return As<Color32?, NullableColor32Converter>(field, converter);
        }

        /// <summary>
        /// 요소의 수가 세 개 또는 네 개인 필드값 목록을 <see cref="Color32?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값 목록이 비어있거나 모든 필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Color32?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">요소의 수가 올바르지 않은 경우.</exception>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        /// <remarks>
        /// 필드값 목록의 요소중 일부만 없다면 해당 요소는 0으로 치환된다.
        /// </remarks>
        public static Color32? AsColor32(this Field[] fields, IFormatProvider provider = null)
        {
            if (fields.Length == 0)
            {
                return null;
            }

            if (fields.Length != 3 && fields.Length != 4)
            {
                throw new CsvReaderException(
                    "필드값 목록의 요소 수가 올바르지 않습니다.",
                    null,
                    fields[0].RecordNumber,
                    GetUniqueColumnNames(fields));
            }

            byte? r = fields[0].AsByte(provider: provider);
            byte? g = fields[1].AsByte(provider: provider);
            byte? b = fields[2].AsByte(provider: provider);
            byte? a = null;

            if (fields.Length == 3)
            {
                if (r == null && g == null && b == null)
                {
                    return null;
                }

                a = 255;
            }
            else if (fields.Length == 4)
            {
                a = fields[3].AsByte(provider: provider);

                if (r == null && g == null && b == null && a == null)
                {
                    return null;
                }
            }

            return new Color32(r ?? 0, g ?? 0, b ?? 0, a ?? 0);
        }

        /// <summary>
        /// 16진수 색상 코드값을 <see cref="Color?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <returns>필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Color?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        public static Color? AsColor(this Field field)
        {
            if (string.IsNullOrEmpty(field.Value))
            {
                return null;
            }

            var converter = new NullableColorConverter();
            return As<Color?, NullableColorConverter>(field, converter);
        }

        /// <summary>
        /// 요소의 수가 세 개 또는 네 개인 필드값 목록을 <see cref="Color?"/> 형식으로 변환하고 값이 없는 경우 <c>null</c>을 반환한다.
        /// </summary>
        /// <param name="provider">필드값의 서식에 대한 문화권 특성을 지정한다. <c>null</c>인 경우 현재 스레드에 설정된 문화권 정보를 사용한다.</param>
        /// <returns>필드값 목록이 비어있거나 모든 필드값이 없다면 <c>null</c>, 그렇지 않다면 <see cref="Color?"/> 형식으로 변환한 값을 반환한다.</returns>
        /// <exception cref="CsvReaderException">요소의 수가 올바르지 않은 경우.</exception>
        /// <exception cref="CsvReaderException">필드값을 변환할 수 없는 경우.</exception>
        /// <remarks>
        /// 필드값 목록의 요소중 일부만 없다면 해당 요소는 0으로 치환된다.
        /// </remarks>
        public static Color? AsColor(this Field[] fields, IFormatProvider provider = null)
        {
            if (fields.Length == 0)
            {
                return null;
            }

            if (fields.Length != 3 && fields.Length != 4)
            {
                throw new CsvReaderException(
                    "필드값 목록의 요소 수가 올바르지 않습니다.",
                    null,
                    fields[0].RecordNumber,
                    GetUniqueColumnNames(fields));
            }

            float? r = fields[0].AsSingle(provider: provider);
            float? g = fields[1].AsSingle(provider: provider);
            float? b = fields[2].AsSingle(provider: provider);
            float? a = null;

            if (fields.Length == 3)
            {
                if (r == null && g == null && b == null)
                {
                    return null;
                }

                a = 1.0f;
            }
            else if (fields.Length == 4)
            {
                a = fields[3].AsSingle(provider: provider);

                if (r == null && g == null && b == null && a == null)
                {
                    return null;
                }
            }

            return new Color(r ?? 0.0f, g ?? 0.0f, b ?? 0.0f, a ?? 0.0f);
        }

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

        private static string GetUniqueColumnNames(Field[] fields)
        {
            var hashSet = new HashSet<string>();
            var uniqueColumnNames = fields
                .Select(f => f.ColumnName)
                .Where(cn => !string.IsNullOrEmpty(cn) && hashSet.Add(cn))
                .ToArray();

            return string.Join(",", uniqueColumnNames);

        }
    }
}
