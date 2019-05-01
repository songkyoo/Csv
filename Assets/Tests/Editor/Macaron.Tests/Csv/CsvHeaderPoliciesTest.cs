using System;
using System.Linq;
using NUnit.Framework;
using Macaron.Csv;
using Macaron.Csv.Iterators;

namespace Macaron.Tests.Csv
{
    [TestFixture]
    public class CsvHeaderPoliciesTest : AssertionHelper
    {
        private static ICsvIterator CreateIterator(string str)
        {
            return new CsvStringIterator(str);
        }

        [TestFixture]
        public class FirstRecordTest : AssertionHelper
        {
            [Test]
            public void CreateHeader_IteratorParamIsNull_ThrowsException()
            {
                var headerPolicy = CsvHeaderPolicies.FirstRecord();

                Assert.Throws(
                    TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("iterator"),
                    () => headerPolicy.CreateHeader(null));
            }

            [Test]
            public void CreateHeader_RecordPropertyOfIteratorParamIsNull_ThrowsException()
            {
                var headerPolicy = CsvHeaderPolicies.FirstRecord();
                var iterator = CreateIterator("Manufacturer,Name\r\nWalther,WA 2000\r\n");

                Assert.Throws(
                    TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("iterator"),
                    () => headerPolicy.CreateHeader(iterator));
            }

            [Test]
            public void CreateHeader_IteratorParamHasRecords_CallsIteratorMoveNext()
            {
                var headerPolicy = CsvHeaderPolicies.FirstRecord();
                var iterator = CreateIterator("Manufacturer,Name\r\nWalther,WA 2000\r\n");

                iterator.MoveNext();
                headerPolicy.CreateHeader(iterator);

                Assert.That(iterator, Property("Record").EqualTo(new[] { "Walther", "WA 2000" }));
            }

            [Test]
            public void CreateHeader_IteratorParamHasRecords_ReturnsHeaderUsingResultOfIteratorCall()
            {
                var headerPolicy = CsvHeaderPolicies.FirstRecord();
                var iterator = CreateIterator("Manufacturer,Name\r\nWalther,WA 2000\r\n");

                iterator.MoveNext();

                var header = headerPolicy.CreateHeader(iterator);

                Assert.That(header, EqualTo(new[] { "Manufacturer", "Name" }));
            }

            [Test]
            public void CreateHeader_SelectorParamIsNotNull_SelectorIsCalledWithEachColumnName()
            {
                var record = new[] { "Manufacturer", "Name" };
                var headerPolicy = CsvHeaderPolicies.FirstRecord(columnName => columnName.ToUpperInvariant());
                var iterator = CreateIterator(string.Join(",", record));

                iterator.MoveNext();

                var header = headerPolicy.CreateHeader(iterator);

                Assert.That(header, EqualTo(record.Select(columnName => columnName.ToUpperInvariant())));
            }
        }

        [TestFixture]
        public class UserDefinedTest : AssertionHelper
        {
            [TestCase("")]
            [TestCase("Walther,WA 2000\r\n")]
            public void CreateHeader_Always_DoesNotCallIteratorMoveNext(string str)
            {
                var userDefinedHeader = new [] { "Manufacturer", "Name" };
                var headerPolicy = CsvHeaderPolicies.UserDefined(userDefinedHeader);
                var iterator = CreateIterator(str);

                headerPolicy.CreateHeader(iterator);

                Assert.That(iterator, Property("Record").Null);
            }

            [TestCase("")]
            [TestCase("Walther,WA 2000\r\n")]
            public void CreateHeader_Always_ReturnsHeaderUsingProvidedValue(string str)
            {
                var userDefinedHeader = new [] { "Manufacturer", "Name" };
                var headerPolicy = CsvHeaderPolicies.UserDefined(userDefinedHeader);
                var iterator = CreateIterator(str);
                var header = headerPolicy.CreateHeader(iterator);

                Assert.That(header, EqualTo(userDefinedHeader));
            }
        }
    }
}
