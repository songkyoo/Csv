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
        /// 첫 번째 레코드를 열 이름 목록으로 사용하고, 특정 열의 인덱스를 얻기 위해 열 이름과 같은 이름을 가지는 <typeparamref name="TEnum"/> 형식의 값을 사용하는 헤더를 생성한다.
        /// </summary>
        /// <param name="selector">첫 번째 레코드를 읽은 후 각 필드 값에 대해 호출된다.</param>
        /// <typeparam name="TEnum">특정 열의 인덱스를 찾기 위해 사용되는 열거형 형식.</typeparam>
        /// <returns>첫 번째 레코드를 열 이름 목록으로 사용하는 헤더를 생성하는 <see cref="ICsvHeaderPolicy{TEnum}"/> 개체를 반환한다.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/>이 열거형 형식이 아닌 경우.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/>의 기본 형식이 <c>byte, sbyte, short, ushort, int</c> 중 하나가 아닌 경우.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/>이 중복된 값을 가지는 경우.</exception>
        /// <remarks>
        /// 열 이름 목록으로 사용될 레코드는 <c>null</c> 요소를 가져서는 안 된다. 빈 문자열은 허용되며 해당 열은 <see cref="ICsvHeader{TEnum}.GetIndex"/>를 호출할 때 무시된다. 또한 레코드의 모든 필드는 중복된 값을 가져서는 안 되며, <typeparamref name="TEnum"/> 형식의 모든 값을 가지고 있어야 한다.
        ///
        /// <typeparamref name="TEnum"/>의 기본 형식은 <c>sbyte, byte, short, ushort, int</c> 중 하나여야 하고 중복된 값을 가져서는 안 된다. <typeparamref name="TEnum"/>의 값이 0부터 시작하고 순차적으로 증가한다면 배열을 사용하여 인덱스를 찾고 <typeparamref name="TEnum"/>의 순서와 헤더의 열 이름 순서가 완전히 일치한다면 열거형의 값을 인덱스로 사용한다. 그 외의 경우에는 <typeparamref name="TEnum"/>을 키 형식으로 가지는 <see cref="Dictionary{TEnum, int}"/>를 사용하여 인덱스를 찾는다.
        /// </remarks>
        public static ICsvHeaderPolicy<TEnum> FirstRecord<TEnum>(Func<string, string> selector = null)
            where TEnum : struct, IConvertible
        {
            return new HeaderPolicies.FirstRecord<TEnum>(selector);
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

        /// <summary>
        /// 열 이름 목록 혹은 <typeparamref name="TEnum"/> 열거형 형식을 사용하고, 특정 열의 인덱스를 얻기 위해 열 이름과 같은 이름을 가지는 <typeparamref name="TEnum"/> 형식의 값을 사용하는 헤더를 생성한다.
        /// </summary>
        /// <param name="columnNames">열 이름 목록. <c>null</c> 요소를 가질 수 없다. <c>null</c>인 경우 열 이름 목록을 <typeparamref name="TEnum"/> 형식으로부터 가져온다.</param>
        /// <typeparam name="TEnum">특정 열의 인덱스를 찾기 위해 사용되는 열거형 형식.</typeparam>
        /// <returns><paramref name="columnNames"/> 또는 <typeparamref name="TEnum"/> 형식을 사용하여 헤더를 생성하는 <see cref="ICsvHeaderPolicy{TEnum}"/> 개체를 반환한다.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/>이 열거형 형식이 아닌 경우.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/>의 기본 형식이 <c>byte, sbyte, short, ushort, int</c> 중 하나가 아닌 경우.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum"/>이 중복된 값을 가지는 경우.</exception>
        /// <exception cref="ArgumentException"><paramref name="columnNames"/>가 <c>null</c>이 아닐 때, <c>null</c> 요소를 포함하고 있는 경우.</exception>
        /// <exception cref="ArgumentException"><paramref name="columnNames"/>가 <c>null</c>이 아닐 때, 중복된 요소를 포함하고 있는 경우.</exception>
        /// <exception cref="ArgumentException"><paramref name="columnNames"/>가 <c>null</c>이 아닐 때, <typeparamref name="TEnum"/> 형식의 모든 값을 가지지 않는 경우.</exception>
        /// <remarks>
        /// <paramref name="columnNames"/>가 <c>null</c>이 아닐 때, <paramref name="columnNames"/>는 <c>null</c> 요소를 가져서는 안 된다. 빈 문자열은 허용되며 해당 열은 <see cref="ICsvHeader{TEnum}.GetIndex"/>를 호출할 때 무시된다. 또한 <paramref name="columnNames"/>의 모든 요소는 중복된 값을 가져서는 안 되며, <typeparamref name="TEnum"/> 형식의 모든 값을 가지고 있어야 한다.
        ///
        /// <typeparamref name="TEnum"/>의 기본 형식은 <c>sbyte, byte, short, ushort, int</c> 중 하나여야 하고 중복된 값을 가져서는 안 된다. <typeparamref name="TEnum"/>의 값이 0부터 시작하고 순차적으로 증가한다면 배열을 사용하여 인덱스를 찾고 <typeparamref name="TEnum"/>의 순서와 헤더의 열 이름 순서가 완전히 일치한다면 열거형의 값을 인덱스로 사용한다. 그 외의 경우에는 <typeparamref name="TEnum"/>을 키 형식으로 가지는 <see cref="Dictionary{TEnum, int}"/>를 사용하여 인덱스를 찾는다.
        /// </remarks>
        public static ICsvHeaderPolicy<TEnum> UserDefined<TEnum>(IList<string> columnNames = null)
            where TEnum : struct, IConvertible
        {
            return new HeaderPolicies.UserDefined<TEnum>(columnNames);
        }
    }
}
