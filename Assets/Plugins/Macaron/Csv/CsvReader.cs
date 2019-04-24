using Macaron.Csv.Internal;
using Macaron.Csv.Internal.HeaderPolicies;
using Macaron.Csv.Iterators;

namespace Macaron.Csv
{
    public static class CsvReader
    {
        public static ICsvReader<int> Create(string str, CsvReaderSettings settings)
        {
            var iterator = CreateIterator(str, settings);
            return new Reader<int>(iterator, Index.Instance, true);
        }

        public static ICsvReader<T> Create<T>(string str, CsvReaderSettings settings, ICsvHeaderPolicy<T> headerPolicy)
        {
            var iterator = CreateIterator(str, settings);
            return new Reader<T>(iterator, headerPolicy, false);
        }

        private static ICsvIterator CreateIterator(string str, CsvReaderSettings settings)
        {
            return new CsvStringIterator(
                str,
                settings.FieldSeparator,
                settings.Quote,
                settings.Escape,
                settings.RecordTerminator,
                settings.TrimMode,
                settings.NullValue);
        }
    }
}
