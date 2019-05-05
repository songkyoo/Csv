using System;
using NUnit.Framework;
using Macaron.Csv;
using Macaron.Csv.Internal;
using Macaron.Csv.Internal.Headers;

namespace Macaron.Tests.Csv.Internal
{
    [TestFixture]
    public class RecordTest : AssertionHelper
    {
        [Test]
        public void Ctor_HeaderParamIsNull_ThrowsException()
        {
            var fields = new[] { "Walther", "WA 2000", "7.62x51mm NATO" };

            Assert.Throws(
                TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("header"),
                () => CreateRecord(null, 1, fields));
        }

        [Test]
        public void Ctor_RecordNumberIsLessThanOne_ThrowsException()
        {
            var header = CreateHeader();
            var fields = new[] { "Walther", "WA 2000", "7.62x51mm NATO" };

            Assert.Throws(
                TypeOf<ArgumentOutOfRangeException>().And.Property("ParamName").EqualTo("recordNumber"),
                () => CreateRecord(header, 0, fields));
        }

        [Test]
        public void Ctor_FieldsParamIsNull_ThrowsException()
        {
            var header = CreateHeader();

            Assert.Throws(
                TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("fields"),
                () => CreateRecord(header, 1, null));
        }

        [Test]
        public void Ctor_FieldsParamIsValid_StoresDuplicatedValues()
        {
            var header = CreateHeader();
            var fields = new[] { "Walther", "WA 2000", "7.62x51mm NATO" };
            var record = CreateRecord(header, 1, fields);

            fields[2] = ".300Winchester Magnum";

            Assert.That(record, EqualTo(new[] { "Walther", "WA 2000", "7.62x51mm NATO" }));
        }

        [Test]
        public void RecordNumber_RecordNumberParamOfCtorIsValid_ReturnsValue()
        {
            var header = CreateHeader();
            var recordNumber = 1;
            var fields = new[] { "Walther", "WA 2000", "7.62x51mm NATO" };
            var record = CreateRecord(header, recordNumber, fields);

            Assert.That(record.RecordNumber, EqualTo(recordNumber));
        }

        [Test]
        public void Item_SetValue_ThrowsException()
        {
            var header = CreateHeader();
            var fields = new[] { "Heckler & Koch", "PSG1", "7.62x51mm NATO" };
            var record = CreateRecord(header, 1, fields);

            Assert.Throws(TypeOf<NotSupportedException>(), () => record[0] = "Denel");
        }

        [Test]
        public void Get_ColumnNameIsValid_ReturnsFieldValue()
        {
            var header = CreateHeader();
            var fields = new[] { "Accuracy International", "AWP", ".243 Winchester" };
            var record = CreateRecord(header, 1, fields);

            Assert.That(record.Get("Manufacturer"), EqualTo(fields[0]));
        }

        [Test]
        public void Get_ColumnNameIsInvalid_ThrowsException()
        {
            var header = CreateHeader();
            var fields = new[] { "Accuracy International", "AWP", ".243 Winchester" };
            var record = CreateRecord(header, 1, fields);
            var invalidColumnName = "Action";

            Assert.Throws(
                TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("columnName"),
                () => record.Get(invalidColumnName));
        }

        private ICsvHeader<string> CreateHeader()
        {
            return new DictHeader(new[] { "Manufacturer", "Name", "Cartridge" }, null);
        }

        private ICsvRecord<string> CreateRecord(ICsvHeader<string> header, int recordNumber, string[] fields)
        {
            return new Record<string>(header, recordNumber, fields);
        }
    }
}
