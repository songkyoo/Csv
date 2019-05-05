using System;
using System.Collections.Generic;

namespace Macaron.Csv
{
    /// <summary>
    /// 필드 값을 가지는 레코드를 나타낸다. 기본적으로 읽기 전용 문자열 배열처럼 동작하며, <see cref="Get"/> 메서드를 통해 지정한 형식으로 필드 값을 가져올 수 있다.
    /// </summary>
    /// <typeparam name="T">열 이름 형식.</typeparam>
    public interface ICsvRecord<T> : IList<string>
    {
        /// <summary>
        /// 레코드 번호.
        /// </summary>
        /// <value>레코드 번호.</value>
        int RecordNumber { get; }

        /// <summary>
        /// 지정한 형식을 열 이름으로 필드 값을 가져온다.
        /// </summary>
        /// <param name="columnName">열 이름.</param>
        /// <returns>지정한 열 이름에 대한 필드 값.</returns>
        /// <exception cref="ArgumentException"><paramref name="columnName"/>이 유효한 열 이름이 아닌 경우.</exception>
        string Get(T columnName);
    }
}
