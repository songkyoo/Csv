using System;

namespace Macaron.Csv
{
    partial class ICsvRecordExtensionMethod
    {
        /// <summary>
        /// 필드와 관련된 정보를 가지는 구조체.
        /// </summary>
        public struct Field : IEquatable<Field>
        {
            /// <summary>
            /// 필드가 속한 레코드 번호.
            /// </summary>
            public readonly int RecordNumber;

            /// <summary>
            /// 필드의 열 이름.
            /// </summary>
            public readonly string ColumnName;

            /// <summary>
            /// 필드의 값.
            /// </summary>
            public readonly string Value;

            public Field(int recordNumber, string columnName, string value)
            {
                RecordNumber = recordNumber;
                ColumnName = columnName;
                Value = value;
            }

            #region Operators
            public static bool operator ==(Field lhs, Field rhs)
            {
                return lhs.Equals(rhs);
            }

            public static bool operator !=(Field lhs, Field rhs)
            {
                return !(lhs == rhs);
            }
            #endregion

            #region Overrides
            public override bool Equals(object obj)
            {
                if (obj is Field)
                {
                    return Equals((Field)obj);
                }

                return false;
            }

            public override int GetHashCode()
            {
                var h1 = RecordNumber;
                var h2 = ColumnName != null ? ColumnName.GetHashCode() : 0;
                var h3 = Value != null ? Value.GetHashCode() : 0;

                unchecked
                {
                    var result = 0;
                    result = (result * 397) ^ h1;
                    result = (result * 397) ^ h2;
                    result = (result * 397) ^ h3;

                    return result;
                }
            }
            #endregion

            #region Implementation of IEquatable<Field>
            public bool Equals(Field other)
            {
                return RecordNumber == other.RecordNumber && ColumnName == other.ColumnName && Value == other.Value;
            }
            #endregion
        }
    }
}
