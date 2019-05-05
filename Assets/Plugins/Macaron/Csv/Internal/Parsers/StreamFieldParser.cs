using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#if UNITY_EDITOR
[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("Assembly-CSharp-Editor")]
#endif
namespace Macaron.Csv.Internal.Parsers
{
    internal partial class StreamFieldParser
    {
        #region Fields
        private readonly int _separator;
        private readonly int? _quote;
        private readonly int? _escape;
        private readonly int[] _specialCharactersForQuotedField;
        private readonly int[] _specialCharactersForUnquotedField;
        private readonly CsvRecordTerminator? _recordTerminator;
        private readonly CsvTrimMode _trimMode;
        private readonly string _nullValue;
        private readonly StringBuilder _value;
        private int _lineNumber;
        private int _linePosition;
        #endregion

        #region Constructors
        public StreamFieldParser(
            char separator,
            char? quote,
            char? escape,
            CsvRecordTerminator? recordTerminator,
            CsvTrimMode trimMode,
            string nullValue)
        {
            if (separator == '\r' || separator == '\n')
            {
                throw new ArgumentException("새줄 문자를 필드 구분자로 사용할 수 없습니다.", "separator");
            }

            if (quote == separator)
            {
                throw new ArgumentException("필드 구분자를 인용부호로 사용할 수 없습니다.", "quote");
            }

            if (quote == '\r' || quote == '\n')
            {
                throw new ArgumentException("새줄 문자를 인용부호로 사용할 수 없습니다.", "quote");
            }

            if (escape == separator)
            {
                throw new ArgumentException("필드 구분자를 이스케이프로 사용할 수 없습니다.", "escape");
            }

            if (escape == '\r' || escape == '\n')
            {
                throw new ArgumentException("새줄 문자를 이스케이프로 사용할 수 없습니다.", "escape");
            }

            if (recordTerminator != null &&
                (recordTerminator < CsvRecordTerminator.CR || recordTerminator > CsvRecordTerminator.CRLF))
            {
                throw new ArgumentOutOfRangeException("recordTerminator");
            }

            if (trimMode < CsvTrimMode.None || trimMode > CsvTrimMode.Both)
            {
                throw new ArgumentOutOfRangeException("trimMode");
            }

            _separator = separator;
            _quote = quote;
            _escape = escape;
            _recordTerminator = recordTerminator;
            _trimMode = trimMode;
            _nullValue = nullValue;

            var specialCharactersForQuotedField = new List<int> { '\r', '\n' };
            var specialCharactersForUnquotedField = new List<int> { separator, '\r', '\n' };

            if (quote != null)
            {
                specialCharactersForQuotedField.Add(quote.Value);
            }

            if (escape != null)
            {
                if (escape != quote)
                {
                    specialCharactersForQuotedField.Add(escape.Value);
                }

                specialCharactersForUnquotedField.Add(escape.Value);
            }

            _specialCharactersForQuotedField = specialCharactersForQuotedField.ToArray(); // \r \n escape quote
            _specialCharactersForUnquotedField = specialCharactersForUnquotedField.ToArray(); // \r \n escape separator

            _value = new StringBuilder();
        }
        #endregion

        #region Methods
        public FieldParsingResult Parse(TextReader reader, int lineNumber, int linePosition)
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
                return new FieldParsingResult
                {
                    Value = _nullValue == string.Empty ? null : string.Empty,
                    Length = 0,
                    IsLast = true,
                    LineNumber = lineNumber,
                    LinePosition = linePosition
                };
            }

            _value.Length = 0;
            _lineNumber = lineNumber;
            _linePosition = linePosition;

            var cacheReader = new CacheTextReader(reader);
            var isQuoted = false;

            if (cacheReader.Next == _quote) // quote가 null이면 실행되지 않는다.
            {
                isQuoted = true;

                // 인용부호.
                cacheReader.Read();
                _linePosition += 1;

                ParseQuotedField(ref cacheReader);

                if (cacheReader.Current == -1)
                {
                    // 닫는 인용부호를 찾을 수 없는 경우 여는 인용부호를 오류 발생 위치로 사용한다.
                    throw new CsvParsingException(
                        "닫는 인용부호를 찾을 수 없습니다.",
                        null,
                        lineNumber,
                        linePosition);
                }

                var ch = cacheReader.Read();
                if (ch != -1 && ch != '\r' && ch != '\n')
                {
                    _linePosition += 1;
                }
            }
            else
            {
                ParseUnquotedField(ref cacheReader);
            }

            var fieldEnd = ParseFieldEnd(ref cacheReader);
            var isLast = fieldEnd != FieldEnd.Separator;

            return new FieldParsingResult
            {
                Value = GetValue(isQuoted),
                End = fieldEnd,
                Length = cacheReader.Length,
                IsLast = isLast,
                LineNumber = _lineNumber,
                LinePosition = _linePosition
            };
        }

        private void IncreaseLineNumber()
        {
            _lineNumber += 1;
            _linePosition = 1;
        }

        private void ParseQuotedField(ref CacheTextReader reader)
        {
            var ch = 0;

            for (; ; )
            {
                ch = reader.Read();

                // 문자열 끝이라면 닫는 인용부호를 찾지 못한 것이다.
                if (ch == -1)
                {
                    return;
                }

                if (Array.IndexOf(_specialCharactersForQuotedField, ch) != -1)
                {
                    break;
                }

                _value.Append((char)ch);
                _linePosition += 1;
            }

            // quote == escape인 경우를 처리하기 위해서 escape인 경우를 먼저 처리한다.
            if (ch == _escape)
            {
                var nextCh = reader.Next;
                if (nextCh == _quote || nextCh == _escape)
                {
                    _value.Append((char)reader.Read());
                    _linePosition += 2;

                    ParseQuotedField(ref reader);
                    return;
                }

                // TODO escapeHandler char -> string
            }

            // 닫는 인용부호.
            if (ch == _quote)
            {
                _linePosition += 1;
                return;
            }

            // 여기서 ch는 escape 또는 \r 또는 \n이다.
            _value.Append((char)ch);

            if (ch == '\r' || ch == '\n')
            {
                IncreaseLineNumber();

                // \r\n은 한 번에 처리.
                if (ch == '\r' && reader.Next == '\n')
                {
                    _value.Append((char)reader.Read());
                }
            }
            else
            {
                _linePosition += 1;
            }

            ParseQuotedField(ref reader);
            return;
        }

        private void ParseUnquotedField(ref CacheTextReader reader)
        {
            var ch = 0;

            for (; ; )
            {
                ch = reader.Read();

                if (ch == -1)
                {
                    return;
                }

                if (Array.IndexOf(_specialCharactersForUnquotedField, ch) != -1)
                {
                    break;
                }

                _value.Append((char)ch);
                _linePosition += 1;
            }

            if (IsEndOfField(ch, reader.Next))
            {
                if (ch == _separator)
                {
                    _linePosition += 1;
                }

                return;
            }

            if (ch == _escape) // escape가 null이면 무시된다.
            {
                var nextCh = reader.Next;

                if (nextCh == -1)
                {
                    throw new CsvParsingException(
                        "유효한 이스케이프 위치가 아닙니다.",
                        null,
                        _lineNumber,
                        _linePosition);
                }

                var isNewline = nextCh == '\r' || nextCh == '\n';
                var isSeparatorOrEscape = nextCh == _separator || nextCh == _escape;
                if (isNewline || isSeparatorOrEscape)
                {
                    ch = reader.Read();

                    if (isSeparatorOrEscape)
                    {
                        _linePosition += 1;
                    }
                }

                // TODO escapeHandler char -> string
            }

            _value.Append((char)ch);

            var prevCh = _value.Length > 1 ? (int?)_value[_value.Length - 2] : null;
            if (ch == '\r' || (ch == '\n' && prevCh != '\r'))
            {
                IncreaseLineNumber();
            }
            else if (ch != '\n')
            {
                _linePosition += 1;
            }

            ParseUnquotedField(ref reader);
            return;
        }

        private FieldEnd ParseFieldEnd(ref CacheTextReader reader)
        {
            var ch = reader.Current;

            if (ch == -1)
            {
                return FieldEnd.EOF;
            }

            if (ch == _separator)
            {
                return FieldEnd.Separator;
            }

            if (ch == '\r')
            {
                if (reader.Next == '\n')
                {
                    reader.Read();
                    return FieldEnd.CRLF;
                }
                else
                {
                    return FieldEnd.CR;
                }
            }

            if (ch == '\n')
            {
                return FieldEnd.LF;
            }

            // 필드는 파일 끝 또는 필드, 레코드 구분자로 끝나야 한다.
            throw new CsvParsingException(
                "필드 구분자 또는 레코드 구분자여야 합니다.",
                null,
                _lineNumber,
                _linePosition - 1);
        }

        private bool IsEndOfField(int ch, int nextCh)
        {
            if (ch == _separator)
            {
                return true;
            }

            switch (_recordTerminator)
            {
                case CsvRecordTerminator.CR: return ch == '\r';
                case CsvRecordTerminator.LF: return ch == '\n';
                case CsvRecordTerminator.CRLF: return ch == '\r' && nextCh == '\n';

                default:
                    return ch == '\r' || ch =='\n';
            }
        }

        private string GetValue(bool isQuoted)
        {
            var startIndex = 0;
            var endIndex = _value.Length;

            // 공백 제거.
            if (!isQuoted)
            {
                if ((_trimMode & CsvTrimMode.Left) != 0)
                {
                    while (startIndex < _value.Length && Char.IsWhiteSpace(_value[startIndex]))
                    {
                        startIndex += 1;
                    }
                }

                if ((_trimMode & CsvTrimMode.Right) != 0)
                {
                    while (endIndex > startIndex && Char.IsWhiteSpace(_value[endIndex - 1]))
                    {
                        endIndex -= 1;
                    }
                }
            }

            // 널 확인.
            var valueLength = endIndex - startIndex;

            if (_nullValue == null || _nullValue.Length != valueLength)
            {
                return _value.ToString(startIndex, valueLength);
            }

            for (int i = 0; i < valueLength; ++i)
            {
                if (_nullValue[i] != _value[startIndex + i])
                {
                    return _value.ToString(startIndex, valueLength);
                }
            }

            return null;
        }
        #endregion
    }
}
