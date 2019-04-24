using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

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

        public CsvParsingException(string message, int lineNumber, int linePosition, Exception inner = null)
            : base(message, inner)
        {
            this._lineNumber = lineNumber;
            this._linePosition = linePosition;
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
                var strs = new List<string>();

                if (_lineNumber > 0)
                {
                    strs.Add("줄 번호: " + _lineNumber.ToString(CultureInfo.InvariantCulture));
                }

                if (_linePosition > 0)
                {
                    strs.Add("위치: " + _lineNumber.ToString(CultureInfo.InvariantCulture));
                }

                if (strs.Count > 0)
                {
                    return string.Format(base.Message + "({0})", string.Join(", ", strs.ToArray()));
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
