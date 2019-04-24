namespace Macaron.Csv
{
    /// <summary>
    /// <see cref="ICsvReader{T}"/>를 구현한 개체에서 헤더를 생성하는 방식을 결정한다.
    /// </summary>
    /// <typeparam name="T"><see cref="ICsvHeader{T}"/>에서 열 인덱스를 얻기 위해 사용되는 형식.</typeparam>
    public interface ICsvHeaderPolicy<T>
    {
        /// <summary>
        /// <see cref="ICsvIterator"/> 개체를 사용하여 헤더를 생성한다.
        /// </summary>
        /// <param name="iterator">헤더를 생성하는데 사용될 <see cref="ICsvIterator"/> 개체.</param>
        /// <returns><paramref name="iterator"/>를 사용하여 생성된 <see cref="ICsvHeader{T}"/> 개체를 반환한다.</returns>
        ICsvHeader<T> CreateHeader(ICsvIterator iterator);
    }
}
