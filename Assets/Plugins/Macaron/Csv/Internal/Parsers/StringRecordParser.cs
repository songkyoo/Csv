using System;
using System.Collections.Generic;

#if UNITY_EDITOR
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Assembly-CSharp-Editor")]
#endif
namespace Macaron.Csv.Internal.Parsers
{
    internal class StringRecordParser
    {
        #region Fields
        private readonly StringFieldParser _fieldParser;
        private readonly List<string> _values;
        private int _index;
        private int _lineNumber;
        private int _linePosition;
        #endregion

        #region Constructors
        public StringRecordParser(StringFieldParser fieldParser)
        {
            if (fieldParser == null)
            {
                throw new ArgumentNullException("fieldParser");
            }

            _fieldParser = fieldParser;
            _values = new List<string>();
        }
        #endregion

        #region Methods
        public RecordParsingResult Parse(string str, int startIndex, int lineNumber, int linePosition)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (startIndex < 0 || startIndex > str.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            if (lineNumber < 1)
            {
                throw new ArgumentOutOfRangeException("lineNumber");
            }

            if (linePosition < 1)
            {
                throw new ArgumentOutOfRangeException("linePosition");
            }

            if (startIndex >= str.Length)
            {
                return new RecordParsingResult
                {
                    #if !UNITY_5_6_OR_NEWER || NET_2_0 || NET_2_0_SUBSET
                    Values = new string[0],
                    #else
                    Values = System.Array.Empty<string>(),
                    #endif
                    Terminator = null,
                    Length = 0,
                    LineNumber = lineNumber,
                    LinePosition = linePosition
                };
            }

            _values.Clear();
            _index = startIndex;
            _lineNumber = lineNumber;
            _linePosition = linePosition;

            while (true)
            {
                var result = _fieldParser.Parse(str, _index, _lineNumber, _linePosition);

                _values.Add(result.Value);
                _index += result.Length;
                _lineNumber = result.LineNumber;
                _linePosition = result.LinePosition;

                if (result.IsLast)
                {
                    break;
                }
            }

            var terminator = default(CsvRecordTerminator?);

            if (_index < str.Length)
            {
                var ch = str[_index];
                switch (ch)
                {
                    case '\r':
                        if (_index + 1 < str.Length && str[_index + 1] == '\n')
                        {
                            terminator = CsvRecordTerminator.CRLF;
                        }
                        else
                        {
                            terminator = CsvRecordTerminator.CR;
                        }
                        break;

                    case '\n':
                        terminator = CsvRecordTerminator.LF;
                        break;

                    // ch가 \r, \n이 아닌 경우는 StringFieldParser에서 예외가 발생하기 때문에 처리하지 않는다.
                }
            }

            var terminatorLength = 0;

            switch (terminator)
            {
                case CsvRecordTerminator.CR:
                case CsvRecordTerminator.LF:
                    terminatorLength += 1;
                    break;

                case CsvRecordTerminator.CRLF:
                    terminatorLength += 2;
                    break;
            }

            var values = _values.ToArray();
            var length = _index - startIndex + terminatorLength;

            return new RecordParsingResult
            {
                Values = values,
                Terminator = terminator,
                Length = length,
                LineNumber = _lineNumber,
                LinePosition = _linePosition
            };
        }
        #endregion
    }
}
