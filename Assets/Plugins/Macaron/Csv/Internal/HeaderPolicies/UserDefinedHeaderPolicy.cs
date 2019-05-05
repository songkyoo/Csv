using System.Collections.Generic;
using Macaron.Csv.Internal.Headers;

namespace Macaron.Csv.Internal.HeaderPolicies
{
    internal class UserDefinedHeaderPolicy : ICsvHeaderPolicy<string>
    {
        private readonly DictHeader _header;

        public UserDefinedHeaderPolicy(IList<string> columnNames, IEqualityComparer<string> comparer)
        {
            _header = new DictHeader(columnNames, comparer);
        }

        public ICsvHeader<string> CreateHeader(ICsvIterator iterator)
        {
            return _header;
        }
    }
}
