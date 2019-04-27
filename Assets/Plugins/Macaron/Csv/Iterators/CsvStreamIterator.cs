using System;
using System.IO;
using Macaron.Csv.Internal.Parsers;

namespace Macaron.Csv.Iterators
{
    /// <summary>
    /// CSV 스트림을 사용하는 <see cref="ICsvIterator"/> 구현.
    /// </summary>
    public class CsvStreamIterator : CsvIterator
    {
        #region Fields
        private TextReader _reader;
        private bool _leaveOpen;
        private StreamRecordParser _recordParser;
        private string[] _fields;
        private CsvRecordTerminator? _recordTerminator;
        private int _lineNumber;
        private int _nextLineNumber;
        private int _linePosition;
        private bool _disposed;
        #endregion

        #region Constructors
        /// <summary>
        /// <see cref="Stream"/> 개체를 사용하여 <see cref="CsvStreamIterator"/> 개체를 생성한다.
        /// </summary>
        /// <param name="stream">CSV 데이터를 가진 <see cref="Stream"/> 개체. BOM이 없다면 UTF8 형식이라고 가정한다.</param>
        /// <param name="fieldSeparator">필드 구분자. <c>'\r'</c>, <c>'\n'</c>은 허용되지 않는다.</param>
        /// <param name="quote">인용부호. <paramref name="fieldSeparator"/>, <c>'\r'</c>, <c>'\n'</c>은 허용되지 않는다.</param>
        /// <param name="escape">이스케이프. <paramref name="fieldSeparator"/>, <c>'\r'</c>, <c>'\n'</c>은 허용되지 않는다.</param>
        /// <param name="recordTerminator">레코드 구분자. <c>null</c>인 경우 첫 번째 레코드의 구분자를 사용한다.</param>
        /// <param name="trimMode">인용부호로 둘러 쌓이지 않은 필드의 좌우 공백 처리 방법.</param>
        /// <param name="nullValue"><c>null</c>이 아닌 경우 필드의 값과 같다면 해당 필드는 <c>null</c>을 반환한다. <paramref name="trimMode"/>가 적용된 이후의 값을 비교한다.</param>
        /// <param name="leaveOpen"><c>false</c>라면 <see cref="Dispose"/> 호출 시 입력 받은 <see cref="Stream"/> 개체의 <see cref="Stream.Dispose"/>를 호출한다.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/>이 <c>null</c>인 경우.</exception>
        /// <exception cref="ArgumentException"><paramref name="fieldSeparator"/>가 <c>'\r'</c>, <c>'\n'</c>인 경우.</exception>
        /// <exception cref="ArgumentException"><paramref name="quote"/>가 <paramref name="fieldSeparator"/>와 같거나 <c>'\r'</c>, <c>'\n'</c>인 경우.</exception>
        /// <exception cref="ArgumentException"><paramref name="escape"/>가 <paramref name="fieldSeparator"/>와 같거나 <c>'\r'</c>, <c>'\n'</c>인 경우.</exception>
        /// <remarks>
        /// <paramref name="recordTerminator"/>가 <c>null</c>일 때, 인용부호를 사용하지 않는 필드가 새줄 문자를 포함하고 있는 경우 올바르게 파싱하지 못한다.
        /// </remarks>
        public CsvStreamIterator(
            Stream stream,
            char fieldSeparator = ',',
            char? quote = '"',
            char? escape = '"',
            CsvRecordTerminator? recordTerminator = null,
            CsvTrimMode trimMode = CsvTrimMode.None,
            string nullValue = null,
            bool leaveOpen = false)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            var reader = new StreamReader(stream);
            Init(reader, leaveOpen, fieldSeparator, quote, escape, recordTerminator, trimMode, nullValue);
        }

        /// <summary>
        /// <see cref="TextReader"/> 개체를 사용하여 <see cref="CsvStreamIterator"/> 개체를 생성한다.
        /// </summary>
        /// <param name="reader">CSV 데이터를 가진 <see cref="TextReader"/> 개체.</param>
        /// <param name="fieldSeparator">필드 구분자. <c>'\r'</c>, <c>'\n'</c>은 허용되지 않는다.</param>
        /// <param name="quote">인용부호. <paramref name="fieldSeparator"/>, <c>'\r'</c>, <c>'\n'</c>은 허용되지 않는다.</param>
        /// <param name="escape">이스케이프. <paramref name="fieldSeparator"/>, <c>'\r'</c>, <c>'\n'</c>은 허용되지 않는다.</param>
        /// <param name="recordTerminator">레코드 구분자. <c>null</c>인 경우 첫 번째 레코드의 구분자를 사용한다.</param>
        /// <param name="trimMode">인용부호로 둘러 쌓이지 않은 필드의 좌우 공백 처리 방법.</param>
        /// <param name="nullValue"><c>null</c>이 아닌 경우 필드의 값과 같다면 해당 필드는 <c>null</c>을 반환한다. <paramref name="trimMode"/>가 적용된 이후의 값을 비교한다.</param>
        /// <param name="leaveOpen"><c>false</c>라면 <see cref="Dispose"/> 호출 시 입력 받은 <see cref="TextReader"/> 개체의 <see cref="TextReader.Dispose"/>를 호출한다.</param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/>가 <c>null</c>인 경우.</exception>
        /// <exception cref="ArgumentException"><paramref name="fieldSeparator"/>가 <c>'\r'</c>, <c>'\n'</c>인 경우.</exception>
        /// <exception cref="ArgumentException"><paramref name="quote"/>가 <paramref name="fieldSeparator"/>와 같거나 <c>'\r'</c>, <c>'\n'</c>인 경우.</exception>
        /// <exception cref="ArgumentException"><paramref name="escape"/>가 <paramref name="fieldSeparator"/>와 같거나 <c>'\r'</c>, <c>'\n'</c>인 경우.</exception>
        /// <remarks>
        /// <paramref name="recordTerminator"/>가 <c>null</c>일 때, 인용부호를 사용하지 않는 필드가 새줄 문자를 포함하고 있는 경우 올바르게 파싱하지 못한다.
        /// </remarks>
        public CsvStreamIterator(
            TextReader reader,
            char fieldSeparator = ',',
            char? quote = '"',
            char? escape = '"',
            CsvRecordTerminator? recordTerminator = null,
            CsvTrimMode trimMode = CsvTrimMode.None,
            string nullValue = null,
            bool leaveOpen = false)
        {
            Init(reader, leaveOpen, fieldSeparator, quote, escape, recordTerminator, trimMode, nullValue);
        }
        #endregion

        #region Overrides
        protected override string[] GetRecord()
        {
            return _fields;
        }

        protected override int GetLineNumber()
        {
            return _lineNumber;
        }

        protected override int GetLinePosition()
        {
            return _linePosition;
        }

        protected override CsvRecordTerminator? GetRecordTerminator()
        {
            return _recordTerminator;
        }

        protected override bool OnMoveNext()
        {
            if (_reader.Peek() == -1)
            {
                _fields = null;
                _recordTerminator = null;

                return false;
            }

            var result = _recordParser.Parse(_reader, _nextLineNumber, 1);
            _fields = result.Values;
            _recordTerminator = result.Terminator;
            _lineNumber = result.LineNumber;
            _nextLineNumber = result.LineNumber + 1;
            _linePosition = result.LinePosition;

            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            try
            {
                if (disposing && !_leaveOpen)
                {
                    _reader.Dispose();
                }
            }
            finally
            {
                _reader = null;
                _recordParser = null;
                _fields = null;

                _disposed = true;

                base.Dispose(disposing);
            }
        }
        #endregion

        private void Init(
            TextReader reader,
            bool leaveOpen,
            char fieldSeparator,
            char? quote,
            char? escape,
            CsvRecordTerminator? recordTerminator,
            CsvTrimMode trimMode,
            string nullValue)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            var fieldParser = new StreamFieldParser(
                fieldSeparator,
                quote,
                escape,
                recordTerminator,
                trimMode,
                nullValue);
            var recordParser = new StreamRecordParser(fieldParser);

            _reader = reader;
            _leaveOpen = leaveOpen;
            _recordParser = recordParser;
            _recordTerminator = recordTerminator;
            _lineNumber = 1;
            _nextLineNumber = 1;
            _linePosition = 1;
        }
    }
}
