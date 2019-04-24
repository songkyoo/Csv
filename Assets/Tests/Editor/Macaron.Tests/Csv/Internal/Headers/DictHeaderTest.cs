using System;
using NUnit.Framework;
using Macaron.Csv.Internal.Headers;

namespace Macaron.Tests.Csv.Internal.Headers
{
    [TestFixture]
    public class DictHeaderTest : AssertionHelper
    {
        [Test]
        public void Ctor_ColumnNamesParamIsNull_ThrowsException()
        {
            Assert.Throws(
                TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("columnNames"),
                () => new DictHeader(null, null));
        }

        [Test]
        public void Ctor_ColumnNamesParamHasNullValue_ThrowsException()
        {
            var columnNames = new[] { "Manufacturer", "Name", null, "Cartridge" };

            Assert.Throws(
                TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("columnNames"),
                () => new DictHeader(columnNames, null));
        }

        [Test]
        public void Ctor_ColumnNamesParamHasDuplicatedValue_ThrowsException()
        {
            var columnNames = new[] { "Manufacturer", "Name", "Manufacturer" };

            Assert.Throws(
                TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("columnNames"),
                () => new DictHeader(columnNames, null));
        }

        [Test]
        public void Ctor_ColumnNamesParamHasDuplicatedValueByComparer_ThrowsException()
        {
            var columnNames = new[] { "Manufacturer", "Name", "manufacturer" };

            Assert.Throws(
                TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("columnNames"),
                () => new DictHeader(columnNames, StringComparer.OrdinalIgnoreCase));
        }

        [Test]
        public void Ctor_ColumnNamesParamHasEmptyValue_DoesNotTreatAsColumnName()
        {
            var columnNames = new[] { "Manufacturer", "Name", string.Empty, "Cartridge" };
            var header = new DictHeader(columnNames, null);

            Assert.That(header.IndexOf(string.Empty), EqualTo(2));
            Assert.That(header.GetIndex(string.Empty), EqualTo(-1));
        }

        [Test]
        public void Item_SetValue_ThrowsException()
        {
            var columnNames = new[] { "Manufacturer", "Name", "Cartridge" };
            var header = new DictHeader(columnNames, null);

            Assert.Throws(TypeOf<NotSupportedException>(), () => header[0] = "Denel");
        }

        [Test]
        public void GetIndex_ColumnNameParamIsNull_ThrowsException()
        {
            var columnNames = new[] { "Manufacturer", "Name" };
            var header = new DictHeader(columnNames, null);

            Assert.Throws(
                TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("columnName"),
                () => header.GetIndex(null));
        }

        [Test]
        public void GetIndex_HeaderContainsColumnNameParam_ReturnsColumnIndex()
        {
            var columnNames = new[] { "Manufacturer", "Name" };
            var header = new DictHeader(columnNames, null);

            Assert.That(header.GetIndex("Name"), EqualTo(1));
        }

        [Test]
        public void GetIndex_HeaderDoesNotContainColumnNameParam_ReturnsMinusOne()
        {
            var columnNames = new[] { "Manufacturer", "Name" };
            var header = new DictHeader(columnNames, null);

            Assert.That(header.GetIndex("name"), EqualTo(-1));
        }

        [Test]
        public void GetIndex_ComparerParamOfCtorIsNotNull_FindsIndexUseComparer()
        {
            var columnNames = new[] { "Manufacturer", "Name" };
            var header = new DictHeader(columnNames, StringComparer.OrdinalIgnoreCase);

            Assert.That(header.GetIndex("name"), EqualTo(1));
        }
    }
}
