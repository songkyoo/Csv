using System;

namespace Macaron.Csv
{
    /// <summary>
    /// 인용부호로 둘러 쌓이지 않은 필드의 좌우 공백 처리 방법을 나타낸다.
    /// </summary>
    [Flags]
    public enum CsvTrimMode
    {
        /// <summary>공백을 제거하지 않는다.</summary>
        None = 0x00,

        /// <summary>왼쪽의 공백을 제거한다.</summary>
        Left = 0x01,

        /// <summary>오른쪽의 공백을 제거한다.</summary>
        Right = 0x02,

        /// <summary>왼쪽와 오른쪽의 공백을 제거한다.</summary>
        Both = Left | Right
    }
}
