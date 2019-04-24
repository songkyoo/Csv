using System;
using System.Collections.Generic;

namespace Macaron.Csv
{
    /// <summary>
    /// CSV 데이터를 레코드 단위로 읽는다.
    /// </summary>
    public interface ICsvReader<T> : IDisposable
    {
        #region Properties
        IList<string> Header { get; }

        ICsvRecord<T> Record { get; }
        #endregion

        #region Methods
        void Close();

        /// <summary>
        /// 다음 레코드를 읽어 <see cref="Record"/>에 할당한다.
        /// </summary>
        /// <returns>레코드를 읽었다면 <c>true</c>를 반환하고, 더 이상 읽을 레코드가 없다면 <c>false</c>를 반환한다.</returns>
        /// <exception cref="CsvParsingException">CSV 데이터에 구문 오류가 있는 경우.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IDisposable.Dispose"/>가 호출된 후에 메서드를 호출한 경우.</exception>
        bool Read();
        #endregion
    }
}
