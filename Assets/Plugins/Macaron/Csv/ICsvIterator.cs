using System;
using System.Collections.Generic;

namespace Macaron.Csv
{
    /// <summary>
    /// CSV 데이터를 읽어 레코드 단위로 열거한다.
    /// </summary>
    public interface ICsvIterator : IDisposable
    {
        /// <summary>
        /// 현재 줄 번호. 레코드 번호와 관계 없이 새줄 문자에 따라 증가한다. 레코드의 시작 위치가 아닌 파싱이 종료된 위치를 나타낸다.
        /// </summary>
        /// <value>현재 줄 번호.</value>
        /// <exception cref="ObjectDisposedException"><see cref="IDisposable.Dispose"/>이 호출된 후 값에 접근한 경우.</exception>
        int LineNumber { get; }

        /// <summary>
        /// 현재 줄에서의 문자 위치. 레코드의 시작 위치가 아닌 파싱이 종료된 위치를 나타낸다.
        /// </summary>
        /// <value>현재 줄에서의 문자 위치.</value>
        /// <exception cref="ObjectDisposedException"><see cref="IDisposable.Dispose"/>이 호출된 후 값에 접근한 경우.</exception>
        int LinePosition { get; }

        /// <summary>
        /// 현재 레코드를 나타낸다.
        /// </summary>
        /// <value>필드 값을 가진 배열.</value>
        string[] Record { get; }

        /// <summary>
        /// 현재 레코드 번호. 최초에는 0, 레코드를 파싱할 때마다 1씩 증가한다.
        /// </summary>
        /// <value>현재 레코드 번호.</value>
        /// <exception cref="ObjectDisposedException"><see cref="IDisposable.Dispose"/>이 호출된 후 값에 접근한 경우.</exception>
        int RecordNumber { get; }

        /// <summary>
        /// 다음 레코드를 읽어 <see cref="Record"/>에 할당한다.
        /// </summary>
        /// <returns>레코드를 읽었다면 <c>true</c>를 반환하고, 더 이상 읽을 레코드가 없다면 <c>false</c>를 반환한다.</returns>
        /// <exception cref="CsvParsingException">파싱 중 구문 오류가 발생한 경우.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="IDisposable.Dispose"/>가 호출된 후에 메서드를 호출한 경우.</exception>
        bool MoveNext();
    }
}
