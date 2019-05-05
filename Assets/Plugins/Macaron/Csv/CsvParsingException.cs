using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Macaron.Csv
{
    /// <summary>
    /// CSV 데이터에 구문 오류가 있는 경우 발생하는 예외.
    /// </summary>
    [Serializable]
    public class CsvParsingException : CsvException
    {
        #region Fields
        private readonly int _lineNumber;
        private readonly int _linePosition;
        #endregion

        #region Constructors
        public CsvParsingException() : base()
        {
        }

        public CsvParsingException(string message) : base(message)
        {
        }

        public CsvParsingException(string message, Exception inner) : base(message, inner)
        {
        }

        public CsvParsingException(string message, Exception inner, int lineNumber, int linePosition)
            : base(message, inner)
        {
            _lineNumber = lineNumber;
            _linePosition = linePosition;
        }

        protected CsvParsingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _lineNumber = info.GetInt32("LineNumber");
            _linePosition = info.GetInt32("LinePosition");
        }
        #endregion

        #region Overrides
        public override string Message
        {
            get
            {
                if (_lineNumber > 0)
                {
                    var builder = new StringBuilder(base.Message);
                    builder.Append("(줄 번호: ");
                    builder.Append(_lineNumber.ToString(CultureInfo.InvariantCulture));

                    if (_linePosition > 0)
                    {
                        builder.Append(", 위치: ");
                        builder.Append(_linePosition.ToString(CultureInfo.InvariantCulture));
                    }

                    return builder.Append(')').ToString();
                }
                else
                {
                    return base.Message;
                }
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("LineNumber", _lineNumber);
            info.AddValue("LinePosition", _linePosition);
        }
        #endregion

        #region Properties
        /// <summary>
        /// 구문 오류가 발생한 줄 번호.
        /// </summary>
        /// <value>줄 번호.</value>
        public int LineNumber
        {
            get { return _lineNumber; }
        }

        /// <summary>
        /// 구문 오류가 발생한 줄에서의 위치.
        /// </summary>
        /// <value>줄 위치.</value>
        public int LinePosition
        {
            get { return _linePosition; }
        }
        #endregion
    }
}
