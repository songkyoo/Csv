using System;
using System.Linq;
using NUnit.Framework;
using Macaron.Csv.Internal.Headers;

namespace Macaron.Tests.Csv.Internal.Headers
{
    [TestFixture]
    public class EnumHeaderTest : AssertionHelper
    {
        #region Enums
        private enum Nothing
        {
        }

        private enum ZeroBasedContinuous
        {
            Manufacturer,
            Name,
            Cartridge
        }

        private enum NonZeroBasedContinuous
        {
            Manufacturer = -1,
            Name = 0,
            Cartridge = 1
        }

        private enum ZeroBasedDiscontinuous
        {
            Manufacturer = 0,
            Name = 2,
            Cartridge = 4
        }

        private enum NonZeroBasedDiscontinuous
        {
            Manufacturer = -2,
            Name = 0,
            Cartridge = 2
        }

        private enum DuplicatedValues
        {
            Manufacturer = 0,
            Name = 0,
            Cartridge = 1
        }

        private enum UnderlyingType_Uint : uint
        {
            Manufacturer,
            Name,
            Cartridge
        }

        private enum UnderlyingType_Long : long
        {
            Manufacturer,
            Name,
            Cartridge
        }

        private enum UnderlyingType_Ulong : ulong
        {
            Manufacturer,
            Name,
            Cartridge
        }
        #endregion

        [Test]
        public void Ctor_ColumnNamesParamIsNull_UsesTEnumValuesForColumnNames()
        {
            var header = new EnumHeader<ZeroBasedContinuous>(null);

            Assert.That(header, EqualTo(new[] { "Manufacturer", "Name", "Cartridge" }));
        }

        [Test]
        public void Ctor_ColumnNamesParamIsNull_HeaderColumnNamesSortedByBinaryValueOfTEnumValues()
        {
            var header = new EnumHeader<NonZeroBasedDiscontinuous>(null);

            Assert.That(header, EqualTo(new[] { "Name", "Cartridge", "Manufacturer" }));
        }

        [Test]
        public void Ctor_ColumnNamesParamHasNullValue_ThrowsException()
        {
            var columnNames = new[] { "Manufacturer", "Name", null, "Cartridge" };

            Assert.Throws(
                TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("columnNames"),
                () => new EnumHeader<Nothing>(columnNames));
        }

        [Test]
        public void Ctor_ColumnNamesParamHasDuplicatedValue_ThrowsException()
        {
            var columnNames = new[] { "Manufacturer", "Name", "Manufacturer" };

            Assert.Throws(
                TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("columnNames"),
                () => new EnumHeader<Nothing>(columnNames));
        }

        [Test]
        public void Ctor_EnumValueIsNotInColumnNames_ThrowsException()
        {
            var columnNames = new[] { "Manufacturer", "Name" };

            Assert.Throws(
                TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("columnNames"),
                () => new EnumHeader<ZeroBasedContinuous>(columnNames));
        }

        [TestCase(default(UnderlyingType_Uint))]
        [TestCase(default(UnderlyingType_Long))]
        [TestCase(default(UnderlyingType_Ulong))]
        public void Ctor_UnderlyingTypeOfTEnumIsInvalid_ThrowsException<T>(T typeInference) where T : struct, IConvertible
        {
            var columnNames = new[] { "Manufacturer", "Name", "Cartridge" };

            Assert.Throws(
                TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("TEnum"),
                () => new EnumHeader<T>(columnNames));
        }

        [Test]
        public void Ctor_TEnumTypeParamHasDuplicatedValues_ThrowsException()
        {
            Assert.Throws(
                TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("TEnum"),
                () => new EnumHeader<DuplicatedValues>(new[] { "Manufacturer", "Name", "Cartridge" }));
        }

        [Test]
        public void Item_SetValue_ThrowsException()
        {
            var columnNames = new[] { "Manufacturer", "Name", "Cartridge" };
            var header = new EnumHeader<ZeroBasedContinuous>(columnNames);

            Assert.Throws(TypeOf<NotSupportedException>(), () => header[0] = "Denel");
        }

        [TestCase(default(ZeroBasedContinuous), "Manufacturer", "Name", "Cartridge")]
        [TestCase(default(NonZeroBasedContinuous), "Manufacturer", "Name", "Cartridge")]
        [TestCase(default(ZeroBasedDiscontinuous), "Manufacturer", "Name", "Cartridge")]
        [TestCase(default(NonZeroBasedDiscontinuous), "Manufacturer", "Name", "Cartridge")]
        public void GetIndex_WithColumnNames_ReturnsColumnIndex<T>(T typeInference, params string[] values) where T : struct, IConvertible
        {
            var names = values.Select(v => (T)Enum.Parse(typeof(T), v)).ToArray();
            var header = new EnumHeader<T>(values);
            var result = names.Select(header.GetIndex).ToArray();

            Assert.That(result, EqualTo(new[] { 0, 1, 2 }));
        }

        [TestCase(default(ZeroBasedContinuous), new[] { "Manufacturer", "Name", "Cartridge" }, new[] { 0, 1, 2 })]
        [TestCase(default(NonZeroBasedContinuous), new[] { "Manufacturer", "Name", "Cartridge" }, new[] { 2, 0, 1 })]
        [TestCase(default(ZeroBasedDiscontinuous), new[] { "Manufacturer", "Name", "Cartridge" }, new[] { 0, 1, 2 })]
        [TestCase(default(NonZeroBasedDiscontinuous), new[] { "Manufacturer", "Name", "Cartridge" }, new[] { 2, 0, 1 })]
        public void GetIndex_WithoutColumnNames_ReturnsColumnIndex<T>(T typeInference, string[] values, int[] indices) where T : struct, IConvertible
        {
            var names = values.Select(v => (T)Enum.Parse(typeof(T), v)).ToArray();
            var header = new EnumHeader<T>(null);
            var result = names.Select(header.GetIndex).ToArray();

            Assert.That(result, EqualTo(indices));
        }
    }
}
