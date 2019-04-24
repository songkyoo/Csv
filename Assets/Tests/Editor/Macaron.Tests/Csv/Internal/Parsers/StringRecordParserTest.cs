using System;
using NUnit.Framework;
using Macaron.Csv;
using Macaron.Csv.Internal.Parsers;

namespace Macaron.Tests.Csv.Internal.Parsers
{
    [TestFixture]
    public class StringRecordParserTest : AssertionHelper
    {
        [Test]
        public void Ctor_FieldParserParamIsNull_ThrowsException()
        {
            Assert.Throws(
                TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("fieldParser"),
                () => new StringRecordParser(null));
        }

        [Test]
        public void Parse_StrParamIsNull_ThrowsException()
        {
            var parser = CreateParser();

            Assert.Throws(
                TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("str"),
                () => parser.Parse(null, 0, 1, 1));
        }

        [Test]
        public void Parse_StartIndexParamIsLessThanZero_ThrowsException()
        {
            var parser = CreateParser();

            Assert.Throws(
                TypeOf<ArgumentOutOfRangeException>().And.Property("ParamName").EqualTo("startIndex"),
                () => parser.Parse(string.Empty, -1, 1, 1));
        }

        [Test]
        public void Parse_StartIndexParamIsGreaterThanLengthOfStrParam_ThrowsException()
        {
            var parser = CreateParser();

            Assert.Throws(
                TypeOf<ArgumentOutOfRangeException>().And.Property("ParamName").EqualTo("startIndex"),
                () => parser.Parse(string.Empty, 1, 1, 1));
        }

        [Test]
        public void Parse_LineNumberParamIsLessThanOne_ThrowsException()
        {
            var parser = CreateParser();

            Assert.Throws(
                TypeOf<ArgumentOutOfRangeException>().And.Property("ParamName").EqualTo("lineNumber"),
                () => parser.Parse(string.Empty, 0, 0, 1));
        }

        [Test]
        public void Parse_LinePositionParamIsLessThanOne_ThrowsException()
        {
            var parser = CreateParser();

            Assert.Throws(
                TypeOf<ArgumentOutOfRangeException>().And.Property("ParamName").EqualTo("linePosition"),
                () => parser.Parse(string.Empty, 0, 1, 0));
        }

        [Test]
        public void Parse_StrParamIsEmpty_LengthOfValuesIsZero()
        {
            var parser = CreateParser();
            var result = parser.Parse(string.Empty, 0, 1, 1);

            Assert.That(result.Values, Length.Zero);
        }

        [TestCase("\r", CsvRecordTerminator.CR)]
        [TestCase("\n", CsvRecordTerminator.LF)]
        [TestCase("\r\n", CsvRecordTerminator.CRLF)]
        public void Parse_StrParamEqualsToRecordTerminator_LengthOfValuesIsOne(string str, CsvRecordTerminator recordTerminator)
        {
            var parser = CreateParser(recordTerminator: recordTerminator);
            var result = parser.Parse(str, 0, 1, 1);

            Assert.That(result.Values, Length.EqualTo(1));
        }

        [TestCase("Denel,NTW-20,Bolt action\nWalther,WA 2000,\"Gas-operated,Rotating bolt\"\n", CsvRecordTerminator.LF, ExpectedResult=25)]
        [TestCase("Accuracy International,AWP,\".243 Winchester\r\n7.62x51mm NATO\"\r\n", CsvRecordTerminator.CRLF, ExpectedResult=62)]
        public int Parse_RecordEndsWithRecordTerminator_ReturnsLengthToNextRecord(string str, CsvRecordTerminator recordTerminator)
        {
            var parser = CreateParser(recordTerminator: recordTerminator);
            var result = parser.Parse(str, 0, 1, 1);

            return result.Length;
        }

        [TestCase("Denel,NTW-20,Bolt action", ExpectedResult=24)]
        [TestCase("Accuracy International,AWP,\".243 Winchester\r\n7.62x51mm NATO\"", ExpectedResult=60)]
        public int Parse_RecordEndsWithEOF_ReturnsLengthToEOF(string str)
        {
            var parser = CreateParser();
            var result = parser.Parse(str, 0, 1, 1);

            return result.Length;
        }

        [TestCase(
            ",",
            ExpectedResult=new[] { "", "" })]
        [TestCase(
            "Denel,NTW-20,Bolt action\r\nWalther,WA 2000,\"Gas-operated,Rotating bolt\"\r\n",
            ExpectedResult=new[] { "Denel", "NTW-20", "Bolt action" })]
        [TestCase(
            "Accuracy International,AWP,\".243 Winchester\r\n7.62x51mm NATO\"",
            ExpectedResult=new[] { "Accuracy International", "AWP", ".243 Winchester\r\n7.62x51mm NATO" })]
        public string[] Parse_RecordConsistsOfMultipleFields_ResturnsValueOfFields(string str)
        {
            var parser = CreateParser();
            var result = parser.Parse(str, 0, 1, 1);

            return result.Values;
        }

        [TestCase("\n\r\n", 283, ExpectedResult=284)]
        [TestCase("Accuracy International,AWP,\".243 Winchester\r\n7.62x51mm NATO\"\r\n", 315, ExpectedResult=316)]
        [TestCase("\"7.62mmx51mm NATO\r.300Winchester Magnum\r7.5x55mm Swiss\",\"Gas-operated,Rotating bolt\"\r\n", 346, ExpectedResult=348)]
        [TestCase("WA 2000,\"7.62x51mm NATO\n.300 Winchester Magnum\n7.5x55mm Swiss\"\r\n", 765, ExpectedResult=767)]
        public int Parse_RecordConsistsOfSingleOrMultipleLines_ReturnsRelativeLineNumberFromLineNumberParam(string str, int lineNumber)
        {
            var parser = CreateParser();
            var result = parser.Parse(str, 0, lineNumber, 1);

            return result.LineNumber;
        }

        [TestCase("\r\n", 315, ExpectedResult=315)]
        [TestCase("Denel,NTW-20,Bolt action", 346, ExpectedResult=370)]
        [TestCase("Walther,WA 2000,\"Gas-operated,Rotating bolt\"", 765, ExpectedResult=809)]
        public int Parse_RecordConsistsOfSingleLine_ReturnsRelativeLinePositionFromLinePositionParam(string str, int linePosition)
        {
            var parser = CreateParser();
            var result = parser.Parse(str, 0, 1, linePosition);

            return result.LinePosition;
        }

        [TestCase("\n\r\n", 283, ExpectedResult=1)]
        [TestCase("Accuracy International,AWP,\".243 Winchester\r\n7.62x51mm NATO\"\r\n", 346, ExpectedResult=16)]
        [TestCase("\"7.62mmx51mm NATO\r.300Winchester Magnum\r7.5x55mm Swiss\",\"Gas-operated,Rotating bolt\"\r\n", 765, ExpectedResult=45)]
        public int Parse_RecordConsistsOfMultipleLines_LinePositionParamNotAffectsToResultLinePosition(string str, int linePosition)
        {
            var parser = CreateParser();
            var result = parser.Parse(str, 0, 1, linePosition);

            return result.LinePosition;
        }

        [TestCase(
            "Accuracy International,AWP\r\n", 22,
            ExpectedResult=new[] { "", "AWP" })]
        [TestCase(
            "Denel,NTW-20,Bolt action\r\nWalther,WA 2000,\"Gas-operated,Rotating bolt\"\r\n", 26,
            ExpectedResult=new[] { "Walther", "WA 2000", "Gas-operated,Rotating bolt" })]
        public string[] Parse_StartIndexParamIsValid_UsesForRecordStartPosition(string str, int startIndex)
        {
            var parser = CreateParser();
            var result = parser.Parse(str, startIndex, 1, 1);

            return result.Values;
        }

        private StringRecordParser CreateParser(
            char separator = ',',
            char? quote = '"',
            char? escape = '"',
            CsvRecordTerminator? recordTerminator = CsvRecordTerminator.CRLF,
            CsvTrimMode trimMode = CsvTrimMode.None,
            string nullValue = null)
        {
            var fieldParser = new StringFieldParser(separator, quote, escape, recordTerminator, trimMode, nullValue);
            return new StringRecordParser(fieldParser);
        }
    }
}
