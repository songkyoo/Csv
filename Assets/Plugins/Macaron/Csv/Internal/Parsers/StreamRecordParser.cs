using System;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Assembly-CSharp-Editor")]
#endif
namespace Macaron.Csv.Internal.Parsers
{
    internal class StreamRecordParser
    {
        #region Fields
        private readonly StreamFieldParser _fieldParser;
        private readonly List<string> _values;
        private int _lineNumber;
        private int _linePosition;
        #endregion

        #region Constructors
        public StreamRecordParser(StreamFieldParser fieldParser)
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
        public RecordParsingResult Parse(TextReader reader, int lineNumber, int linePosition)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (lineNumber < 1)
            {
                throw new ArgumentOutOfRangeException("lineNumber");
            }

            if (linePosition < 1)
            {
                throw new ArgumentOutOfRangeException("linePosition");
            }

            if (reader.Peek() == -1)
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
            _lineNumber = lineNumber;
            _linePosition = linePosition;

            var length = 0;
            var terminator = default(CsvRecordTerminator?);

            while (true)
            {
                var result = _fieldParser.Parse(reader, _lineNumber, _linePosition);

                _values.Add(result.Value);
                length += result.Length;
                _lineNumber = result.LineNumber;
                _linePosition = result.LinePosition;

                if (result.End != FieldEnd.Separator)
                {
                    switch (result.End)
                    {
                        case FieldEnd.CR: terminator = CsvRecordTerminator.CR; break;
                        case FieldEnd.LF: terminator = CsvRecordTerminator.LF; break;
                        case FieldEnd.CRLF: terminator = CsvRecordTerminator.CRLF; break;
                    }

                    break;
                }
            }

            return new RecordParsingResult
            {
                Values = _values.ToArray(),
                Terminator = terminator,
                Length = length,
                LineNumber = _lineNumber,
                LinePosition = _linePosition
            };
        }
        #endregion
    }
}
