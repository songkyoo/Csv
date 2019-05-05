using System;
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
        private readonly string _columnName;
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

        public CsvReaderException(string message, Exception inner, int recordNumber, string columnName)
            : base(message, inner)
        {
            _recordNumber = recordNumber;
            _columnName = columnName;
        }

        protected CsvReaderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _columnName = info.GetString("ColumnName");
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
                    var builder = new StringBuilder(base.Message);
                    builder.Append("(레코드 번호: ");
                    builder.Append(_recordNumber.ToString());

                    if (!string.IsNullOrEmpty(_columnName))
                    {
                        builder.Append(", 열 이름: ");
                        builder.Append(_columnName);
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

            info.AddValue("ColumnName", _columnName);
            info.AddValue("RecordNumber", _recordNumber);
        }
        #endregion

        #region Properties
        public string ColumnName
        {
            get { return _columnName; }
        }

        public int RecordNumber
        {
            get { return _recordNumber; }
        }
        #endregion
    }
}
