using System;
using System.Collections.Generic;
using System.Linq;
using Macaron.Csv.Internal.Headers;

namespace Macaron.Csv.Internal.HeaderPolicies
{
    internal class FirstRecordHeaderPolicy : ICsvHeaderPolicy<string>
    {
        private readonly Func<string, string> _selector;
        private readonly IEqualityComparer<string> _comparer;

        public FirstRecordHeaderPolicy(Func<string, string> selector, IEqualityComparer<string> comparer)
        {
            _selector = selector;
            _comparer = comparer;
        }

        public ICsvHeader<string> CreateHeader(ICsvIterator iterator)
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

            var columnNames = _selector != null ? record.Select(_selector).ToArray() : record;
            iterator.MoveNext();

            return new DictHeader(columnNames, _comparer);
        }
    }
}
