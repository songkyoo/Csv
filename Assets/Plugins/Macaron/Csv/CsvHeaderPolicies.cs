using System;
using System.Collections.Generic;
using HeaderPolicies = Macaron.Csv.Internal.HeaderPolicies;

namespace Macaron.Csv
{
    /// <summary>
    /// 기본 헤더 정책을 구현한 개체를 생성하는 정적 메서드를 가진 정적 클래스.
    /// </summary>
    public static class CsvHeaderPolicies
    {
        /// <summary>
        /// 첫 번째 레코드를 열 이름 목록으로 사용하는 헤더를 생성한다.
        /// </summary>
        /// <param name="selector">첫 번째 레코드를 읽은 후 각 필드 값에 대해 호출된다.</param>
        /// <param name="comparer"><see cref="ICsvHeader{string}.GetIndex"/>를 호출할 때, 열 이름에 대한 비교를 수행하는 방식을 결정한다.</param>
        /// <returns>첫 번째 레코드를 열 이름 목록으로 사용하는 헤더를 생성하는 <see cref="ICsvHeaderPolicy{string}"/> 개체를 반환한다.</returns>
        /// <remarks>
        /// 열 이름 목록으로 사용될 레코드는 <c>null</c> 요소를 가져서는 안 된다. 빈 문자열은 허용되며 해당 열은 <see cref="ICsvHeader{string}.GetIndex"/>를 호출할 때 무시된다. 또한 레코드의 모든 필드는 중복된 값을 가져서는 안 된다.
        /// </remarks>
        public static ICsvHeaderPolicy<string> FirstRecord(
            Func<string, string> selector = null,
            IEqualityComparer<string> comparer = null)
        {
            return new HeaderPolicies.FirstRecord(selector, comparer);
        }

        /// <summary>
        /// 열 이름 목록을 사용하여 헤더를 생성한다.
        /// </summary>
        /// <param name="columnNames">열 이름 목록. <c>null</c>이거나 <c>null</c> 요소를 가질 수 없다.</param>
        /// <param name="comparer"><see cref="ICsvHeader{string}.GetIndex"/>를 호출할 때, 열 이름에 대한 비교를 수행하는 방식을 결정한다.</param>
        /// <returns><paramref name="columnNames"/>를 사용하여 헤더를 생성하는 <see cref="ICsvHeaderPolicy{string}"/> 개체를 반환한다.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="columnNames"/>가 <c>null</c>인 경우.</exception>
        /// <exception cref="ArgumentException"><paramref name="columnNames"/>가 <c>null</c> 요소를 포함하고 있는 경우.</exception>
        /// <exception cref="ArgumentException"><paramref name="columnNames"/>가 중복된 요소를 포함하고 있는 경우.</exception>
        /// <remarks>
        /// <paramref name="columnNames"/>는 <c>null</c> 요소를 가져서는 안 된다. 빈 문자열은 허용되며 해당 열은 <see cref="ICsvHeader{string}.GetIndex"/>를 호출할 때 무시된다. 또한 <paramref name="columnNames"/>의 모든 요소는 중복된 값을 가져서는 안 된다.
        /// </remarks>
        public static ICsvHeaderPolicy<string> UserDefined(
            IList<string> columnNames,
            IEqualityComparer<string> comparer = null)
        {
            return new HeaderPolicies.UserDefined(columnNames, comparer);
        }
    }
}
