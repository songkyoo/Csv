using System;
using System.Runtime.Serialization;

namespace Macaron.Csv
{
    /// <summary>
    /// 일반적인 CSV 관련 오류를 나타내는 예외.
    /// </summary>
    public class CsvException : Exception
    {
        #region Constructors
        public CsvException() : base()
        {
        }

        public CsvException(string message) : base(message)
        {
        }

        public CsvException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CsvException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        #endregion
    }
}
