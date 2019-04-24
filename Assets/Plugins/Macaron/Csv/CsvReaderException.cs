using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace Macaron.Csv
{
    [Serializable]
    public class CsvReaderException : CsvException
    {
        #region Fields
        private readonly int _recordNumber;
        #endregion

        #region Constructors
        public CsvReaderException() : base()
        {
        }

        public CsvReaderException(string message) : base(message)
        {
        }

        public CsvReaderException(string message, Exception inner) : base(message, inner)
        {
        }

        public CsvReaderException(string message, int recordNumber, Exception inner = null) : base(message, inner)
        {
            _recordNumber = recordNumber;
        }

        protected CsvReaderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _recordNumber = info.GetInt32("RecordNumber");
        }
        #endregion

        #region Overrides
        public override string Message
        {
            get
            {
                if (_recordNumber > 0)
                {
                    return string.Format(
                        "{0}({1})",
                        base.Message,
                        "레코드 번호: " + _recordNumber.ToString(CultureInfo.InvariantCulture));
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

            info.AddValue("RecordNumber", _recordNumber);
        }
        #endregion

        #region Properties
        public int RecordNumber
        {
            get { return _recordNumber; }
        }
        #endregion
    }
}
