using System;
using System.Collections.Generic;
using System.Text;

#if UNITY_EDITOR
[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("Assembly-CSharp-Editor")]
#endif
namespace Macaron.Csv.Internal.Parsers
{
    internal class StringFieldParser
    {
        #region Fields
        private readonly char _separator;
        private readonly char? _quote;
        private readonly char? _escape;
        private readonly char[] _specialCharactersForQuotedField;
        private readonly char[] _specialCharactersForUnquotedField;
        private readonly CsvRecordTerminator? _recordTerminator;
        private readonly CsvTrimMode _trimMode;
        private readonly string _nullValue;
        private readonly StringBuilder _value;
        private int _lineNumber;
        private int _linePosition;
        #endregion

        #region Constructors
        public StringFieldParser(
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

            var specialCharactersForQuotedField = new List<char> { '\r', '\n' };
            var specialCharactersForUnquotedField = new List<char> { separator, '\r', '\n' };

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
        public FieldParsingResult Parse(string str, int startIndex, int lineNumber, int linePosition)
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

            if (startIndex == str.Length)
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

            int endIndex;
            bool isQuoted = false;

            if (str[startIndex] == _quote) // quote가 null이면 실행되지 않는다.
            {
                _linePosition += 1;
                endIndex = ParseQuotedField(str, startIndex + 1);
                isQuoted = true;

                if (endIndex == -1)
                {
                    // 닫는 인용부호를 찾을 수 없는 경우 여는 인용부호를 오류 발생 위치로 사용한다.
                    throw new CsvParsingException("닫는 인용부호를 찾을 수 없습니다.", lineNumber, linePosition);
                }

                _linePosition += 1;
                endIndex += 1;
            }
            else
            {
                endIndex = ParseUnquotedField(str, startIndex);
            }

            // 문자열이 끝나지 않았다면 구분자가 와야 한다.
            if (endIndex < str.Length)
            {
                var ch = str[endIndex];
                if (ch != _separator && ch != '\r' && ch != '\n')
                {
                    throw new CsvParsingException(
                        "필드 구분자 또는 레코드 구분자여야 합니다.",
                        _lineNumber,
                        _linePosition);
                }
            }

            var isLast = endIndex >= str.Length || str[endIndex] != _separator;
            var separatorLength = isLast ? 0 : 1;

            return new FieldParsingResult
            {
                Value = GetValue(isQuoted),
                Length = endIndex - startIndex + separatorLength,
                IsLast = isLast,
                LineNumber = _lineNumber,
                LinePosition = _linePosition + separatorLength
            };
        }

        private void IncreaseLineNumber()
        {
            _lineNumber += 1;
            _linePosition = 1;
        }

        private int ParseQuotedField(string str, int startIndex)
        {
            var index = str.IndexOfAny(_specialCharactersForQuotedField, startIndex);

            // 문자열 끝이라면 닫는 인용부호를 찾지 못한 것이다.
            if (index == -1)
            {
                return -1;
            }

            _value.Append(str, startIndex, index - startIndex);
            _linePosition += index - startIndex;

            var ch = str[index];
            var nextCh = index + 1 < str.Length ? (char?)str[index + 1] : null;

            // quote == escape인 경우를 처리하기 위해서 escape인 경우를 먼저 처리한다.
            if (ch == _escape)
            {
                if (nextCh == _quote || nextCh == _escape)
                {
                    _value.Append(nextCh);
                    _linePosition += 2;

                    return ParseQuotedField(str, index + 2);
                }

                // TODO escapeHandler char -> string
            }

            // 닫는 인용부호.
            if (ch == _quote)
            {
                return index;
            }

            // 여기서 ch는 \r 혹은 \n이다.
            _value.Append(ch);
            IncreaseLineNumber();

            // \r\n은 한 번에 처리.
            if (ch == '\r' && nextCh == '\n')
            {
                _value.Append('\n');
                index += 1;
            }

            return ParseQuotedField(str, index + 1);
        }

        private int ParseUnquotedField(string str, int startIndex)
        {
            var index = str.IndexOfAny(_specialCharactersForUnquotedField, startIndex);

            // 문자열 끝.
            if (index == -1)
            {
                index = str.Length;
            }

            _value.Append(str, startIndex, index - startIndex);
            _linePosition += index - startIndex;

            if (index >= str.Length)
            {
                return str.Length;
            }

            var ch = str[index];
            var nextCh = index + 1 < str.Length ? (char?)str[index + 1] : null;

            if (IsEndOfField(ch, nextCh))
            {
                return index;
            }

            if (ch == _escape) // escape가 null이면 무시된다.
            {
                if (nextCh == null)
                {
                    throw new CsvParsingException("유효한 이스케이프 위치가 아닙니다.", _lineNumber, _linePosition);
                }

                if (nextCh == '\r' || nextCh == '\n' || nextCh == _separator || nextCh == _escape)
                {
                    ch = nextCh.Value;
                    index += 1;
                    _linePosition += 1;
                }

                // TODO escapeHandler char -> string
            }

            _value.Append(ch);

            var prevCh = _value.Length > 1 ? (char?)_value[_value.Length - 2] : null;
            if (ch == '\r' || (ch == '\n' && prevCh != '\r'))
            {
                IncreaseLineNumber();
            }
            else if (ch != '\n')
            {
                _linePosition += 1;
            }

            return ParseUnquotedField(str, index + 1);
        }

        private bool IsEndOfField(char ch, char? nextCh)
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
