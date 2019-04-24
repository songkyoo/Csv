using System.Collections.Generic;

namespace Macaron.Csv
{
    /// <summary>
    /// 열 이름 목록을 나타내며, <typeparamref name="T"/>형식의 열 이름에 대한 인덱스를 찾을 수 있는 메서드를 가진다.
    /// </summary>
    /// <typeparam name="T">열 이름을 나타내는 형식.</typeparam>
    /// <remarks>
    /// <see cref="IList{T}.IndexOf"/>는 열 이름의 일치 여부에 대한 비교를 수행한다. <see cref="ICsvHeader{T}.GetIndex"/>가 문자열을 입력받는 경우 구현에 따라 <see cref="IList{T}.IndexOf"/>와 동작이 같지 않을 수 있다.
    /// </remarks>
    public interface ICsvHeader<T> : IList<string>
    {
        /// <summary>
        /// 열 이름에 대한 인덱스를 반환한다.
        /// </summary>
        /// <param name="columnName">열 이름.</param>
        /// <returns>일치하는 열 이름이 있다면 인덱스를 반환하고 찾을 수 없다면 -1을 반환한다.</returns>
        int GetIndex(T columnName);
    }
}
