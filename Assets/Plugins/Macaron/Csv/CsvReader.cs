using System.IO;
using Macaron.Csv.Internal;
using Macaron.Csv.Internal.HeaderPolicies;
using Macaron.Csv.Iterators;

namespace Macaron.Csv
{
    /// <summary>
    /// CSV 데이터를 사용하여 <see cref="ICsvReader{T}"/> 개체를 생성한다.
    /// </summary>
    public static class CsvReader
    {
        /// <summary>
        /// CSV 문자열을 사용하여 헤더가 없는 <see cref="ICsvReader{T}"/> 개체를 생성한다.
        /// </summary>
        /// <param name="str">CSV 문자열.</param>
        /// <param name="settings">생성할 개체에 사용될 설정.</param>
        /// <returns>CSV 문자열을 사용하고 헤더가 없는 <see cref="ICsvReader{T}"/> 개체.</returns>
        /// <remarks>
        /// 생성된 <see cref="ICsvReader{T}"/> 개체의 <see cref="ICsvReader{T}.Header"/> 속성은 <c>null</c>을 반환한다.
        /// </remarks>
        public static ICsvReader<int> Create(string str, CsvReaderSettings settings)
        {
            var iterator = CreateIterator(str, settings);
            return new Reader<int>(iterator, Index.Instance, true);
        }

        /// <summary>
        /// CSV 문자열을 사용하여 <see cref="ICsvReader{T}"/> 개체를 생성한다.
        /// </summary>
        /// <param name="str">CSV 문자열.</param>
        /// <param name="settings">생성할 개체에 사용될 설정.</param>
        /// <param name="headerPolicy">헤더 생성 정책.</param>
        /// <typeparam name="T">헤더에서 특정 열 인덱스를 찾기 위해 사용되는 형식.</typeparam>
        /// <returns>CSV 문자열을 사용하는 <see cref="ICsvReader{T}"/> 개체.</returns>
        public static ICsvReader<T> Create<T>(string str, CsvReaderSettings settings, ICsvHeaderPolicy<T> headerPolicy)
        {
            var iterator = CreateIterator(str, settings);
            return new Reader<T>(iterator, headerPolicy, false);
        }

        /// <summary>
        /// <see cref="Stream"/> 개체를 사용하여 헤더가 없는 <see cref="ICsvReader{T}"/> 개체를 생성한다.
        /// </summary>
        /// <param name="stream">CSV 데이터를 가진 <see cref="Stream"/> 개체. BOM이 없다면 UTF8 형식이라고 가정한다.</param>
        /// <param name="settings">생성할 개체에 사용될 설정.</param>
        /// <param name="leaveOpen"><c>false</c>라면 <see cref="ICsvReader{T}.Close"/> 혹은 <see cref="ICsvReader{T}.Dispose"/> 호출 시 입력 받은 <see cref="Stream"/> 개체의 <see cref="Stream.Dispose"/>를 호출한다.</param>
        /// <returns><see cref="Stream"/>을 사용하고 헤더가 없는 <see cref="ICsvReader{T}"/> 개체.</returns>
        /// <remarks>
        /// 생성된 <see cref="ICsvReader{T}"/> 개체의 <see cref="ICsvReader{T}.Header"/> 속성은 <c>null</c>을 반환한다.
        /// </remarks>
        public static ICsvReader<int> Create(Stream stream, CsvReaderSettings settings, bool leaveOpen = false)
        {
            var iterator = CreateIterator(stream, settings, leaveOpen);
            return new Reader<int>(iterator, Index.Instance, true);
        }

        /// <summary>
        /// <see cref="Stream"/> 개체를 사용하여 <see cref="ICsvReader{T}"/> 개체를 생성한다.
        /// </summary>
        /// <param name="stream">CSV 데이터를 가진 <see cref="Stream"/> 개체. BOM이 없다면 UTF8 형식이라고 가정한다.</param>
        /// <param name="settings">생성할 개체에 사용될 설정.</param>
        /// <param name="headerPolicy">헤더 생성 정책.</param>
        /// <param name="leaveOpen"><c>false</c>라면 <see cref="ICsvReader{T}.Close"/> 혹은 <see cref="ICsvReader{T}.Dispose"/> 호출 시 입력 받은 <see cref="Stream"/> 개체의 <see cref="Stream.Dispose"/>를 호출한다.</param>
        /// <typeparam name="T">헤더에서 특정 열 인덱스를 찾기 위해 사용되는 형식.</typeparam>
        /// <returns><see cref="Stream"/>을 사용하는 <see cref="ICsvReader{T}"/> 개체.</returns>
        public static ICsvReader<T> Create<T>(
            Stream stream,
            CsvReaderSettings settings,
            ICsvHeaderPolicy<T> headerPolicy,
            bool leaveOpen = false)
        {
            var iterator = CreateIterator(stream, settings, leaveOpen);
            return new Reader<T>(iterator, headerPolicy, false);
        }

        /// <summary>
        /// <see cref="TextReader"/> 개체를 사용하여 헤더가 없는 <see cref="ICsvReader{T}"/> 개체를 생성한다.
        /// </summary>
        /// <param name="reader">CSV 데이터를 가진 <see cref="TextReader"/> 개체.</param>
        /// <param name="settings">생성할 개체에 사용될 설정.</param>
        /// <param name="leaveOpen"><c>false</c>라면 <see cref="ICsvReader{T}.Close"/> 혹은 <see cref="ICsvReader{T}.Dispose"/> 호출 시 입력 받은 <see cref="TextReader"/> 개체의 <see cref="TextReader.Dispose"/>를 호출한다.</param>
        /// <returns><see cref="TextReader"/>를 사용하고 헤더가 없는 <see cref="ICsvReader{T}"/> 개체.</returns>
        /// <remarks>
        /// 생성된 <see cref="ICsvReader{T}"/> 개체의 <see cref="ICsvReader{T}.Header"/> 속성은 <c>null</c>을 반환한다.
        /// </remarks>
        public static ICsvReader<int> Create(TextReader reader, CsvReaderSettings settings, bool leaveOpen = false)
        {
            var iterator = CreateIterator(reader, settings, leaveOpen);
            return new Reader<int>(iterator, Index.Instance, true);
        }

        /// <summary>
        /// <see cref="TextReader"/> 개체를 사용하여 <see cref="ICsvReader{T}"/> 개체를 생성한다.
        /// </summary>
        /// <param name="reader">CSV 데이터를 가진 <see cref="TextReader"/> 개체.</param>
        /// <param name="settings">생성할 개체에 사용될 설정.</param>
        /// <param name="headerPolicy">헤더 생성 정책.</param>
        /// <param name="leaveOpen"><c>false</c>라면 <see cref="ICsvReader{T}.Close"/> 혹은 <see cref="ICsvReader{T}.Dispose"/> 호출 시 입력 받은 <see cref="TextReader"/> 개체의 <see cref="TextReader.Dispose"/>를 호출한다.</param>
        /// <typeparam name="T">헤더에서 특정 열 인덱스를 찾기 위해 사용되는 형식.</typeparam>
        /// <returns><see cref="TextReader"/>를 사용하는 <see cref="ICsvReader{T}"/> 개체.</returns>
        public static ICsvReader<T> Create<T>(
            TextReader reader,
            CsvReaderSettings settings,
            ICsvHeaderPolicy<T> headerPolicy,
            bool leaveOpen = false)
        {
            var iterator = CreateIterator(reader, settings, leaveOpen);
            return new Reader<T>(iterator, headerPolicy, false);
        }

        private static ICsvIterator CreateIterator(string str, CsvReaderSettings settings)
        {
            return new CsvStringIterator(
                str,
                settings.FieldSeparator,
                settings.Quote,
                settings.Escape,
                settings.RecordTerminator,
                settings.TrimMode,
                settings.NullValue);
        }

        private static ICsvIterator CreateIterator(Stream stream, CsvReaderSettings settings, bool leaveOpen)
        {
            return new CsvStreamIterator(
                stream,
                settings.FieldSeparator,
                settings.Quote,
                settings.Escape,
                settings.RecordTerminator,
                settings.TrimMode,
                settings.NullValue,
                leaveOpen);
        }

        private static ICsvIterator CreateIterator(TextReader reader, CsvReaderSettings settings, bool leaveOpen)
        {
            return new CsvStreamIterator(
                reader,
                settings.FieldSeparator,
                settings.Quote,
                settings.Escape,
                settings.RecordTerminator,
                settings.TrimMode,
                settings.NullValue,
                leaveOpen);
        }
    }
}
