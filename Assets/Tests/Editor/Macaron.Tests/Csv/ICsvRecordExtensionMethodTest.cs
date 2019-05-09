using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using Macaron.Csv;
using Macaron.Csv.Internal;
using Macaron.Csv.Internal.Headers;

namespace Macaron.Tests.Csv
{
    [TestFixture]
    public class ICsvRecordExtensionMethodTest : AssertionHelper
    {
        [Test]
        public void ParseAsBoolean_FieldValueIsValid_ReturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Boolean", "TRUE" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsBoolean();

            Assert.That(result, True);
        }

        [Test]
        public void ParseAsBoolean_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Boolean", "NotBoolean" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsBoolean());
        }

        [Test]
        public void ParseAsBoolean_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Boolean", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsBoolean();

            Assert.That(result, Null);
        }

        [Test]
        public void ParseAsByte_FieldValueIsValid_ReturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Byte", byte.MaxValue.ToString() };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsByte();

            Assert.That(result, EqualTo(byte.MaxValue));
        }

        [Test]
        public void ParseAsByte_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Byte", "NotNumber" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsByte());
        }

        [Test]
        public void ParseAsByte_StylesAndProviderParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Byte", "$100" };
            var record = CreateRecord(columnNames, 1, fields);
            var styles = NumberStyles.Currency;
            var provider = CultureInfo.GetCultureInfo("en-US");

            var result = record.Parse("Value").AsByte(styles: styles, provider: provider);

            Assert.That(result, EqualTo(100));
        }

        [Test]
        public void ParseAsByte_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Byte", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsByte();

            Assert.That(result, Null);
        }

        [Test]
        public void ParseAsChar_FieldValueIsValid_ReturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Char", "A" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsChar();

            Assert.That(result, EqualTo('A'));
        }

        [Test]
        public void ParseAsChar_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Char", "NotChar" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsChar());
        }

        [Test]
        public void ParseAsChar_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Char", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsChar();

            Assert.That(result, Null);
        }

        [Test]
        public void ParseAsDecimal_FieldValueIsValid_ReturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Decimal", decimal.MinValue.ToString() };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsDecimal();

            Assert.That(result, EqualTo(decimal.MinValue));
        }

        [Test]
        public void ParseAsDecimal_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Decimal", "NotNumber" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsDecimal());
        }

        [Test]
        public void ParseAsDecimal_StylesAndProviderParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Decimal", "$1,000.0" };
            var record = CreateRecord(columnNames, 1, fields);
            var styles = NumberStyles.Currency;
            var provider = CultureInfo.GetCultureInfo("en-US");

            var result = record.Parse("Value").AsDecimal(styles: styles, provider: provider);

            Assert.That(result, EqualTo(1000.0));
        }

        [Test]
        public void ParseAsDecimal_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Decimal", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsDecimal();

            Assert.That(result, Null);
        }

        [Test]
        public void ParseAsDouble_FieldValueIsValid_ReturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Double", double.MinValue.ToString("R") };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsDouble();

            Assert.That(result, EqualTo(double.MinValue));
        }

        [Test]
        public void ParseAsDouble_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Double", "NotNumber" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsDouble());
        }

        [Test]
        public void ParseAsDouble_StylesAndProviderParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Double", "$1,000.0" };
            var record = CreateRecord(columnNames, 1, fields);
            var styles = NumberStyles.Currency;
            var provider = CultureInfo.GetCultureInfo("en-US");

            var result = record.Parse("Value").AsDouble(styles: styles, provider: provider);

            Assert.That(result, EqualTo(1000.0));
        }

        [Test]
        public void ParseAsDouble_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Double", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsDouble();

            Assert.That(result, Null);
        }

        [Test]
        public void ParseAsInt16_FieldValueIsValid_ReturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Int16", short.MinValue.ToString() };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsInt16();

            Assert.That(result, EqualTo(short.MinValue));
        }

        [Test]
        public void ParseAsInt16_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Int16", "NotNumber" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsInt16());
        }

        [Test]
        public void ParseAsInt16_StylesAndProviderParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Int16", "$1,000" };
            var record = CreateRecord(columnNames, 1, fields);
            var styles = NumberStyles.Currency;
            var provider = CultureInfo.GetCultureInfo("en-US");

            var result = record.Parse("Value").AsInt16(styles: styles, provider: provider);

            Assert.That(result, EqualTo(1000));
        }

        [Test]
        public void ParseAsInt16_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Int16", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsInt16();

            Assert.That(result, Null);
        }

        [Test]
        public void ParseAsInt32_FieldValueIsValid_ReturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Int32", int.MinValue.ToString() };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsInt32();

            Assert.That(result, EqualTo(int.MinValue));
        }

        [Test]
        public void ParseAsInt32_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Int32", "NotNumber" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsInt32());
        }

        [Test]
        public void ParseAsInt32_StylesAndProviderParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Int32", "$1,000" };
            var record = CreateRecord(columnNames, 1, fields);
            var styles = NumberStyles.Currency;
            var provider = CultureInfo.GetCultureInfo("en-US");

            var result = record.Parse("Value").AsInt32(styles: styles, provider: provider);

            Assert.That(result, EqualTo(1000));
        }

        [Test]
        public void ParseAsInt32_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Int32", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsInt32();

            Assert.That(result, Null);
        }

        [Test]
        public void ParseAsInt64_FieldValueIsValid_ReturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Int64", long.MinValue.ToString() };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsInt64();

            Assert.That(result, EqualTo(long.MinValue));
        }

        [Test]
        public void ParseAsInt64_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Int64", "NotNumber" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsInt64());
        }

        [Test]
        public void ParseAsInt64_StylesAndProviderParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Int64", "$1,000" };
            var record = CreateRecord(columnNames, 1, fields);
            var styles = NumberStyles.Currency;
            var provider = CultureInfo.GetCultureInfo("en-US");

            var result = record.Parse("Value").AsInt64(styles: styles, provider: provider);

            Assert.That(result, EqualTo(1000));
        }

        [Test]
        public void ParseAsInt64_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Int64", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsInt64();

            Assert.That(result, Null);
        }

        [Test]
        public void ParseAsSByte_FieldValueIsValid_ReturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "SByte", byte.MinValue.ToString() };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsSByte();

            Assert.That(result, EqualTo(byte.MinValue));
        }

        [Test]
        public void ParseAsSByte_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "SByte", "NotNumber" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsSByte());
        }

        [Test]
        public void ParseAsSByte_StylesAndProviderParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "SByte", "$100" };
            var record = CreateRecord(columnNames, 1, fields);
            var styles = NumberStyles.Currency;
            var provider = CultureInfo.GetCultureInfo("en-US");

            var result = record.Parse("Value").AsSByte(styles: styles, provider: provider);

            Assert.That(result, EqualTo(100));
        }

        [Test]
        public void ParseAsSByte_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "SByte", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsSByte();

            Assert.That(result, Null);
        }

        [Test]
        public void ParseAsSingle_FieldValueIsValid_ReturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Single", float.MinValue.ToString("R") };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsSingle();

            Assert.That(result, EqualTo(float.MinValue));
        }

        [Test]
        public void ParseAsSingle_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Single", "NotNumber" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsSingle());
        }

        [Test]
        public void ParseAsSingle_StylesAndProviderParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Single", "$1,000.0" };
            var record = CreateRecord(columnNames, 1, fields);
            var styles = NumberStyles.Currency;
            var provider = CultureInfo.GetCultureInfo("en-US");

            var result = record.Parse("Value").AsSingle(styles: styles, provider: provider);

            Assert.That(result, EqualTo(1000.0f));
        }

        [Test]
        public void ParseAsSingle_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Single", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsSingle();

            Assert.That(result, Null);
        }

        [Test]
        public void ParseAsUInt16_FieldValueIsValid_ReturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "UInt16", ushort.MaxValue.ToString() };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsUInt16();

            Assert.That(result, EqualTo(ushort.MaxValue));
        }

        [Test]
        public void ParseAsUInt16_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "UInt16", "NotNumber" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsUInt16());
        }

        [Test]
        public void ParseAsUInt16_StylesAndProviderParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "UInt16", "$1,000" };
            var record = CreateRecord(columnNames, 1, fields);
            var styles = NumberStyles.Currency;
            var provider = CultureInfo.GetCultureInfo("en-US");

            var result = record.Parse("Value").AsUInt16(styles: styles, provider: provider);

            Assert.That(result, EqualTo(1000));
        }

        [Test]
        public void ParseAsUInt16_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "UInt16", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsUInt16();

            Assert.That(result, Null);
        }

        [Test]
        public void ParseAsUInt32_FieldValueIsValid_ReturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "UInt32", uint.MaxValue.ToString() };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsUInt32();

            Assert.That(result, EqualTo(uint.MaxValue));
        }

        [Test]
        public void ParseAsUInt32_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "UInt32", "NotNumber" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsUInt32());
        }

        [Test]
        public void ParseAsUInt32_StylesAndProviderParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "UInt32", "$1,000" };
            var record = CreateRecord(columnNames, 1, fields);
            var styles = NumberStyles.Currency;
            var provider = CultureInfo.GetCultureInfo("en-US");

            var result = record.Parse("Value").AsUInt32(styles: styles, provider: provider);

            Assert.That(result, EqualTo(1000));
        }

        [Test]
        public void ParseAsUInt32_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "UInt32", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsUInt32();

            Assert.That(result, Null);
        }

        [Test]
        public void ParseAsUInt64_FieldValueIsValid_ReturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "UInt64", ulong.MaxValue.ToString() };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsUInt64();

            Assert.That(result, EqualTo(ulong.MaxValue));
        }

        [Test]
        public void ParseAsUInt64_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "UInt64", "NotNumber" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsUInt64());
        }

        [Test]
        public void ParseAsUInt64_StylesAndProviderParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "UInt64", "$1,000" };
            var record = CreateRecord(columnNames, 1, fields);
            var styles = NumberStyles.Currency;
            var provider = CultureInfo.GetCultureInfo("en-US");

            var result = record.Parse("Value").AsUInt64(styles: styles, provider: provider);

            Assert.That(result, EqualTo(1000));
        }

        [Test]
        public void ParseAsUInt64_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "UInt64", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsUInt64();

            Assert.That(result, Null);
        }

        [Test]
        public void ParseAsEnum_FieldValueIsValid_ReturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "NumberStyles", "Float, AllowThousands" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsEnum<NumberStyles>();

            Assert.That(result, EqualTo(NumberStyles.Float | NumberStyles.AllowThousands));
        }

        [Test]
        public void ParseAsEnum_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "NumberStyles", "NotNumberStyles" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsEnum<NumberStyles>());
        }

        [Test]
        public void ParseAsEnum_IgnoreCaseParamIsTrue_AllowsLowerCaseValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "NumberStyles", "float, allowthousands" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsEnum<NumberStyles>(ignoreCase: true);

            Assert.That(result, EqualTo(NumberStyles.Float | NumberStyles.AllowThousands));
        }

        [Test]
        public void ParseAsEnum_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "NumberStyles", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsEnum<NumberStyles>();

            Assert.That(result, Null);
        }

        [Test]
        public void ParseAsDateTime_FieldValueIsValid_ReturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "DateTime", "Thu, 01 May 2008 07:34:42" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsDateTime();

            Assert.That(result, EqualTo(new DateTime(2008, 5, 1, 7, 34, 42)));
        }

        [Test]
        public void ParseAsDateTime_FormatParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "DateTime", "06/15/2008" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsDateTime(format: "d", provider: CultureInfo.InvariantCulture);

            Assert.That(result, EqualTo(new DateTime(2008, 6, 15)));
        }

        [Test]
        public void ParseAsDateTime_FormatsParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "DateTime", "2009-06-15T13:45:30.0000000Z" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsDateTime(
                formats: new[] { "d", "o" },
                provider: CultureInfo.InvariantCulture,
                styles: DateTimeStyles.AdjustToUniversal);

            Assert.That(result, EqualTo(new DateTime(2009, 6, 15, 13, 45, 30, DateTimeKind.Utc)));
        }

        [Test]
        public void ParseAsDateTime_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "DateTime", "NotDateTime" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsDateTime());
        }

        [Test]
        public void ParseAsDateTime_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "DateTime", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsDateTime();

            Assert.That(result, Null);
        }

        [Test]
        public void ParseAsTimeSpan_FieldValueIsValid_ResturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "TimeSpan", "6.12:14:45.348" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsTimeSpan();

            Assert.That(result, EqualTo(new TimeSpan(6, 12, 14, 45, 348)));
        }

        [Test]
        public void ParseAsTimeSpan_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "TimeSpan", "NotTimeSpan" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsTimeSpan());
        }

        [Test]
        public void ParseAsTimeSpan_FieldValueIsNullOrEmpty_ResturnsDefaultValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "TimeSpan", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsTimeSpan();

            Assert.That(result, Null);
        }

#if UNITY_5_6_OR_NEWER && !NET_2_0 && !NET_2_0_SUBSET
        [Test]
        public void ParseAsTimeSpan_FormatParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "TimeSpan", "17:14:48" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsTimeSpan(
                format: "g",
                provider: CultureInfo.InvariantCulture);

            Assert.That(result, EqualTo(new TimeSpan(17, 14, 48)));
        }

        [Test]
        public void ParseAsTimeSpan_FormatsParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "TimeSpan", "17:14" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsTimeSpan(
                formats: new[] { @"h\:mm\:ss\.fff", @"h\:mm" },
                provider: CultureInfo.InvariantCulture);

            Assert.That(result, EqualTo(new TimeSpan(17, 14, 0)));
        }
#endif

        [Test]
        public void ParseAsDateTimeOffset_FieldValueIsValid_ReturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "DateTimeOffset", "Thu, 01 May 2008 07:34:42" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsDateTimeOffset();

            Assert.That(result, EqualTo(new DateTimeOffset(new DateTime(2008, 5, 1, 7, 34, 42))));
        }

        [Test]
        public void ParseAsDateTimeOffset_FormatParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "DateTimeOffset", "06/15/2008" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsDateTimeOffset(
                format: "d",
                provider: CultureInfo.InvariantCulture,
                styles: DateTimeStyles.AssumeUniversal);

            Assert.That(result, EqualTo(new DateTimeOffset(new DateTime(2008, 6, 15, 0, 0, 0, DateTimeKind.Utc))));
        }

        [Test]
        public void ParseAsDateTimeOffset_FormatsParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "DateTimeOffset", "Sun 15 Jun 2008 8:30 AM -06:00" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsDateTimeOffset(
                formats: new[] { "d", "ddd dd MMM yyyy h:mm tt zzz" },
                provider: CultureInfo.InvariantCulture);

            Assert.That(result, EqualTo(new DateTimeOffset(2008, 6, 15, 8, 30, 00, new TimeSpan(-6, 0, 0))));
        }

        [Test]
        public void ParseAsDateTimeOffset_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "DateTimeOffset", "NotDateTimeOffset" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsDateTimeOffset());
        }

        [Test]
        public void ParseAsDateTimeOffset_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "DateTimeOffset", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsDateTimeOffset();

            Assert.That(result, Null);
        }

        [Test]
        public void ParseAsGuid_FieldValueIsValid_ReturnsParsedValue()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Guid", "1fe528d8-d5e8-4060-b5f9-1608702bd56f" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsGuid();

            Assert.That(result, EqualTo(new Guid("1fe528d8-d5e8-4060-b5f9-1608702bd56f")));
        }

#if UNITY_5_6_OR_NEWER && !NET_2_0 && !NET_2_0_SUBSET
        [Test]
        public void ParseAsGuid_FormatParamIsValid_UsesForParsing()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Guid", "{0xCA761232,0xED42,0x11CE,{0xBA,0xCD,0x00,0xAA,0x00,0x57,0xB2,0x23}}" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsGuid(format: "X");

            Assert.That(result, EqualTo(new Guid("CA761232-ED42-11CE-BACD-00AA0057B223")));
        }
#endif

        [Test]
        public void ParseAsGuid_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Guid", "NotGuid" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsGuid());
        }

        [Test]
        public void ParseAsGuid_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Guid", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsGuid();

            Assert.That(result, Null);
        }

        [Test]
        public void ParseAsUri_FieldValueIsValid_ReturnsParsedValue()
        {
            var uri = "abc://username:password@example.com:123/path/data?key=value&key2=value2#fragid1";
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Uri", uri };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsUri();

            Assert.That(result, EqualTo(new Uri(uri)));
        }

        [Test]
        public void ParseAsUri_FieldValueIsInvalid_ThrowsException()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Uri", "NotUri" };
            var record = CreateRecord(columnNames, 1, fields);

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("ColumnName").EqualTo("Value"),
                () => record.Parse("Value").AsUri());
        }

        [Test]
        public void ParseAsUri_FormatParamIsValid_UsesForParsing()
        {
            var uri = "/index.html";
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Uri", uri };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsUri(uriKind: UriKind.Relative);

            Assert.That(result, EqualTo(new Uri(uri, UriKind.Relative)));
        }

        [Test]
        public void ParseAsUri_FieldValueIsNullOrEmpty_ReturnsNull()
        {
            var columnNames = new[] { "Type", "Value" };
            var fields = new[] { "Uri", "" };
            var record = CreateRecord(columnNames, 1, fields);

            var result = record.Parse("Value").AsUri();

            Assert.That(result, Null);
        }

        private ICsvRecord<string> CreateRecord(string[] columnNames, int recordNumber, string[] fields)
        {
            var header = new DictHeader(columnNames, null);
            return new Record<string>(header, recordNumber, fields);
        }
    }
}
