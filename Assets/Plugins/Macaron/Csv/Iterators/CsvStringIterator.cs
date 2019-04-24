using System;
using Macaron.Csv.Internal.Parsers;

namespace Macaron.Csv.Iterators
{
    /// <summary>
    /// CSV 문자열을 입력받는 <see cref="ICsvIterator"/> 구현.
    /// </summary>
    public class CsvStringIterator : CsvIterator
    {
        private string _str;
        private StringRecordParser _recordParser;
        private int _index;
        private string[] _fields;
        private CsvRecordTerminator? _recordTerminator;
        private int _lineNumber;
        private int _nextLineNumber;
        private int _linePosition;
        private bool _disposed;

        /// <summary>
        /// CSV 문자열을 입력받아 <see cref="CsvIterator"/> 개체를 생성한다.
        /// </summary>
        /// <param name="str">CSV 문자열.</param>
        /// <param name="fieldSeparator">필드 구분자. <c>'\r'</c>, <c>'\n'</c>은 허용되지 않는다.</param>
        /// <param name="quote">인용부호. <paramref name="fieldSeparator"/>, <c>'\r'</c>, <c>'\n'</c>은 허용되지 않는다.</param>
        /// <param name="escape">이스케이프. <paramref name="fieldSeparator"/>, <c>'\r'</c>, <c>'\n'</c>은 허용되지 않는다.</param>
        /// <param name="recordTerminator">레코드 구분자. <c>null</c>인 경우 첫 번째 레코드의 구분자를 사용한다.</param>
        /// <param name="trimMode">인용부호로 둘러 쌓이지 않은 필드의 좌우 공백 처리 방법.</param>
        /// <param name="nullValue"><c>null</c>이 아닌 경우 필드의 값과 같다면 해당 필드는 <c>null</c>을 반환한다. <paramref name="trimMode"/>가 적용된 이후의 값을 비교한다.</param>
        /// <exception cref="ArgumentNullException"><paramref name="str"/>이 <c>null</c>인 경우.</exception>
        /// <exception cref="ArgumentException"><paramref name="fieldSeparator"/>가 <c>'\r'</c>, <c>'\n'</c>인 경우.</exception>
        /// <exception cref="ArgumentException"><paramref name="quote"/>가 <paramref name="fieldSeparator"/>와 같거나 <c>'\r'</c>, <c>'\n'</c>인 경우.</exception>
        /// <exception cref="ArgumentException"><paramref name="escape"/>가 <paramref name="fieldSeparator"/>와 같거나 <c>'\r'</c>, <c>'\n'</c>인 경우.</exception>
        /// <remarks>
        /// <paramref name="recordTerminator"/>가 <c>null</c>일 때, 인용부호를 사용하지 않는 필드가 새줄 문자를 포함하고 있는 경우 올바르게 파싱하지 못한다.
        /// </remarks>
        public CsvStringIterator(
            string str,
            char fieldSeparator = ',',
            char? quote = '"',
            char? escape = '"',
            CsvRecordTerminator? recordTerminator = null,
            CsvTrimMode trimMode = CsvTrimMode.None,
            string nullValue = null)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            var fieldParser = new StringFieldParser(
                fieldSeparator,
                quote,
                escape,
                recordTerminator,
                trimMode,
                nullValue);
            var recordParser = new StringRecordParser(fieldParser);

            _str = str;
            _recordParser = recordParser;
            _recordTerminator = recordTerminator;
            _lineNumber = 1;
            _nextLineNumber = 1;
            _linePosition = 1;
        }

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
            if (_index >= _str.Length)
            {
                _fields = null;
                _recordTerminator = null;
                return false;
            }

            var result = _recordParser.Parse(_str, _index, _nextLineNumber, 1);
            _index += result.Length;
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

            _str = null;
            _recordParser = null;
            _fields = null;

            _disposed = true;

            base.Dispose(disposing);
        }
        #endregion
    }
}
