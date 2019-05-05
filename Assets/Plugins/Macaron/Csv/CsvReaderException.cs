using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Macaron.Csv
{
    /// <summary>
    /// <see cref="ICsvReader{T}"/>에서 발생한 일반적인 오류를 나타내는 예외.
    /// </summary>
    [Serializable]
    public class CsvReaderException : CsvException
    {
        #region Fields
        private readonly int _fieldNumber;
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

        public CsvReaderException(string message, Exception inner, int recordNumber, int fieldNumber)
            : base(message, inner)
        {
            _recordNumber = recordNumber;
            _fieldNumber = fieldNumber;
        }

        protected CsvReaderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _recordNumber = info.GetInt32("RecordNumber");
            _fieldNumber = info.GetInt32("FieldNumber");
        }
        #endregion

        #region Overrides
        public override string Message
        {
            get
            {
                if (_recordNumber > 0)
                {
                    var builder = new StringBuilder(base.Message);
                    builder.Append("(레코드 번호: ");
                    builder.Append(_recordNumber.ToString(CultureInfo.InvariantCulture));

                    if (_fieldNumber > 0)
                    {
                        builder.Append(", 필드 번호: ");
                        builder.Append(_fieldNumber.ToString(CultureInfo.InvariantCulture));
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

            info.AddValue("RecordNumber", _recordNumber);
            info.AddValue("FieldNumber", _fieldNumber);
        }
        #endregion

        #region Properties
        public int FieldNumber
        {
            get { return _fieldNumber; }
        }

        public int RecordNumber
        {
            get { return _recordNumber; }
        }
        #endregion
    }
}
