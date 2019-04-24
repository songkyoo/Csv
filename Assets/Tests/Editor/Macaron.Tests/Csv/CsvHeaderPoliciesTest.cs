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

        [TestFixture]
        public class FirstRecordTEnumTest : AssertionHelper
        {
            private enum ColumnName
            {
                Manufacturer,
                Name,
                Cartridge
            }

            [Test]
            public void CreateHeader_IteratorParamIsNull_ThrowsException()
            {
                var headerPolicy = CsvHeaderPolicies.FirstRecord<ColumnName>();

                Assert.Throws(
                    TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("iterator"),
                    () => headerPolicy.CreateHeader(null));
            }

            [Test]
            public void RecordPropertyOfIteratorParamIsNull_ThrowsException()
            {
                var headerPolicy = CsvHeaderPolicies.FirstRecord<ColumnName>();
                var iterator = CreateIterator("Manufacturer,Name,Cartridge\r\nWalther,WA 2000,7.62x51mm NATO\r\n");

                Assert.Throws(
                    TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("iterator"),
                    () => headerPolicy.CreateHeader(iterator));
            }

            [Test]
            public void CreateHeader_IteratorParamHasRecords_CallsIteratorMoveNext()
            {
                var headerPolicy = CsvHeaderPolicies.FirstRecord<ColumnName>();
                var iterator = CreateIterator("Manufacturer,Name,Cartridge\r\nWalther,WA 2000,7.62x51mm NATO\r\n");

                iterator.MoveNext();
                headerPolicy.CreateHeader(iterator);

                Assert.That(iterator, Property("Record").EqualTo(new[] { "Walther", "WA 2000", "7.62x51mm NATO" }));
            }

            [Test]
            public void CreateHeader_IteratorParamHasRecords_ReturnsHeaderUsingResultOfIteratorCall()
            {
                var headerPolicy = CsvHeaderPolicies.FirstRecord<ColumnName>();
                var iterator = CreateIterator("Manufacturer,Name,Cartridge\r\nWalther,WA 2000,7.62x51mm NATO\r\n");

                iterator.MoveNext();

                var header = headerPolicy.CreateHeader(iterator);

                Assert.That(header, EqualTo(new[] { "Manufacturer", "Name", "Cartridge" }));
            }

            [Test]
            public void CreateHeader_SelectorParamIsNotNull_SelectorIsCalledWithEachColumnName()
            {
                var record = new[] { "manufacturer", "name", "cartridge" };
                var headerPolicy = CsvHeaderPolicies.FirstRecord<ColumnName>(columnName => char.ToUpperInvariant(columnName[0]) + columnName.Substring(1));
                var iterator = CreateIterator(string.Join(",", record));

                iterator.MoveNext();

                var header = headerPolicy.CreateHeader(iterator);

                Assert.That(header, EqualTo(new[] { "Manufacturer", "Name", "Cartridge" }));
            }
        }


        [TestFixture]
        public class UserDefinedTEnumTest : AssertionHelper
        {
            private enum ColumnName
            {
                Manufacturer,
                Name,
                Cartridge
            }

            [TestCase("")]
            [TestCase("Walther,WA 2000\r\n")]
            public void CreateHeader_Always_DoesNotCallIteratorMoveNext(string str)
            {
                var userDefinedHeader = new [] { "Manufacturer", "Name", "Cartridge" };
                var headerPolicy = CsvHeaderPolicies.UserDefined<ColumnName>(userDefinedHeader);
                var iterator = CreateIterator(str);

                headerPolicy.CreateHeader(iterator);

                Assert.That(iterator, Property("Record").Null);
            }

            [TestCase("")]
            [TestCase("Walther,WA 2000,7.62x51mm NATO\r\n")]
            public void CreateHeader_ColumnNamesParamOfCtorIsNotNull_ReturnsHeaderUsingProvidedColumnNames(string str)
            {
                var userDefinedHeader = new [] { "Manufacturer", "Name", "Cartridge" };
                var headerPolicy = CsvHeaderPolicies.UserDefined<ColumnName>(userDefinedHeader);
                var iterator = CreateIterator(str);
                var header = headerPolicy.CreateHeader(iterator);

                Assert.That(header, EqualTo(userDefinedHeader));
            }

            [TestCase("")]
            [TestCase("Walther,WA 2000,7.62x51mm NATO\r\n")]
            public void CreateHeader_ColumnNamesParamOfCtorIsNull_ReturnsHeaderUsingTEnumTypeParamValues(string str)
            {
                var headerPolicy = CsvHeaderPolicies.UserDefined<ColumnName>(null);
                var iterator = CreateIterator(str);
                var header = headerPolicy.CreateHeader(iterator);

                Assert.That(header, EqualTo(new[] { "Manufacturer", "Name", "Cartridge" }));
            }
        }
    }
}
