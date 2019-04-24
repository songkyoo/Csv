using System;
using System.Collections.Generic;
using Macaron.Csv.Internal.Headers;

namespace Macaron.Csv.Internal.HeaderPolicies
{
    internal class UserDefined<TEnum> : ICsvHeaderPolicy<TEnum> where TEnum : struct, IConvertible
    {
        private readonly EnumHeader<TEnum> _header;

        public UserDefined(IList<string> columnNames)
        {
            _header = new EnumHeader<TEnum>(columnNames);
        }

        public ICsvHeader<TEnum> CreateHeader(ICsvIterator iterator)
        {
            return _header;
        }
    }
}
