namespace Macaron.Csv
{
    /// <summary>
    /// <see cref="ICsvReader{T}"/>를 구현한 개체의 동작과 관련된 설정을 가진다.
    /// </summary>
    public partial struct CsvReaderSettings
    {
        /// <summary>
        /// 필드 구분자. <c>'\r'</c>, <c>'\n'</c>은 허용되지 않는다.
        /// </summary>
        public char FieldSeparator;

        /// <summary>
        /// 인용부호. <see cref="FieldSeparator"/>, <c>'\r'</c>, <c>'\n'</c>은 허용되지 않는다.
        /// </summary>
        public char? Quote;

        /// <summary>
        /// 이스케이프. <see cref="FieldSeparator"/>, <c>'\r'</c>, <c>'\n'</c>은 허용되지 않는다.
        /// </summary>
        public char? Escape;

        /// <summary>
        /// 레코드 구분자. <c>null</c>인 경우 첫 번째 레코드의 구분자를 사용한다.
        /// </summary>
        /// <remarks>
        /// <see cref="RecordTerminator"/>가 <c>null</c>일 때, 인용부호를 사용하지 않는 필드가 새줄 문자를 포함하고 있는 경우 올바르게 파싱하지 못한다.
        /// </remarks>
        public CsvRecordTerminator? RecordTerminator;

        /// <summary>
        /// 인용부호로 둘러 쌓이지 않은 필드의 좌우 공백 처리 방법.
        /// </summary>
        public CsvTrimMode TrimMode;

        /// <summary>
        /// <c>null</c>이 아닌 경우 필드의 값과 같다면 해당 필드는 <c>null</c>을 반환한다.
        /// </summary>
        /// <remarks>
        /// <see cref="TrimMode"/>가 적용된 이후의 값을 비교한다.
        /// </remarks>
        public string NullValue;
    }
}
