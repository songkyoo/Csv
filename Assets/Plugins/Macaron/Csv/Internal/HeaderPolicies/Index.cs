using System;
using Macaron.Csv.Internal.Headers;

namespace Macaron.Csv.Internal.HeaderPolicies
{
    internal class Index : ICsvHeaderPolicy<int>
    {
        public static readonly Index Instance = new Index();

        private Index()
        {
            // 외부 생성을 금지.
        }

        public ICsvHeader<int> CreateHeader(ICsvIterator iterator)
        {
            if (iterator == null)
            {
                throw new ArgumentNullException("iterator");
            }

            var record = iterator.Record;

            if (record == null)
            {
                throw new ArgumentException("Record 속성이 null이 아니어야 합니다.", "iterator");
            }

            return new IndexHeader(record.Length);
        }
    }
}
