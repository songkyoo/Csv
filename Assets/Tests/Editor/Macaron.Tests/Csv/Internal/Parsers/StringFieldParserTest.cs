using System;
using NUnit.Framework;
using Macaron.Csv;
using Macaron.Csv.Internal.Parsers;

namespace Macaron.Tests.Csv.Internal.Parsers
{
    [TestFixture]
    public class StringFieldParserTest : AssertionHelper
    {
        [TestCase('\r')]
        [TestCase('\n')]
        public void Ctor_SeparatorParamEqualsToNewline_ThrowsException(char separator)
        {
            Assert.Throws(
                TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("separator"),
                () => CreateParser(separator: separator));
        }

        [TestCase('"')]
        [TestCase('\'')]
        public void Ctor_QuoteParamEqualsToSeparatorParam_ThrowsException(char quote)
        {
            Assert.Throws(
                TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("quote"),
                () => CreateParser(separator: quote, quote: quote));
        }

        [TestCase('\r')]
        [TestCase('\n')]
        public void Ctor_QuoteParamEqualsToNewline_ThrowsException(char quote)
        {
            Assert.Throws(
                TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("quote"),
                () => CreateParser(quote: quote));
        }

        [TestCase(',')]
        [TestCase('\t')]
        public void Ctor_EscapeParamEqualsToSeparatorParam_ThrowsException(char escape)
        {
            Assert.Throws(
                TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("escape"),
                () => CreateParser(separator: escape, escape: escape));
        }

        [TestCase('\r')]
        [TestCase('\n')]
        public void Ctor_EscapeParamEqualsToNewline_ThrowsException(char escape)
        {
            Assert.Throws(
                TypeOf<ArgumentException>().And.Property("ParamName").EqualTo("escape"),
                () => CreateParser(escape: escape));
        }

        [TestCase((CsvRecordTerminator)(-1))]
        [TestCase((CsvRecordTerminator)(3))]
        public void Ctor_RecordTerminatorParamaIsNotValid_ThrowsException(CsvRecordTerminator recordTerminator)
        {
            Assert.Throws(
                TypeOf<ArgumentOutOfRangeException>().And.Property("ParamName").EqualTo("recordTerminator"),
                () => CreateParser(recordTerminator: recordTerminator));
        }


        [TestCase((CsvTrimMode)(-1))]
        [TestCase((CsvTrimMode)(4))]
        public void Ctor_TrimModeParamaIsNotValid_ThrowsException(CsvTrimMode trimMode)
        {
            Assert.Throws(
                TypeOf<ArgumentOutOfRangeException>().And.Property("ParamName").EqualTo("trimMode"),
                () => CreateParser(trimMode: trimMode));
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

        [TestCase(",PSG1", ',', ExpectedResult=1)]
        [TestCase("Heckler & Koch\tPSG1", '\t', ExpectedResult=15)]
        [TestCase("\"\",\".338 Lapua Magnum\"", ',', ExpectedResult=3)]
        [TestCase("\"Noreen \"\"Bad News\"\" ULR 338\"\t\".338 Lapua Magnum\"", '\t', ExpectedResult=30)]
        [TestCase("\"Gas-operated,Rotating bolt\",\"Semi-automatic\"", ',', ExpectedResult=29)]
        public int Parse_NotLastField_ReturnsLengthToNextField(string str, char separator)
        {
            var parser = CreateParser(separator: separator);
            var result = parser.Parse(str, 0, 1, 1);

            return result.Length;
        }

        [TestCase("\r\nPSG1\r\n", CsvRecordTerminator.CRLF, ExpectedResult=0)]
        [TestCase("\nPSG1\n", CsvRecordTerminator.LF, ExpectedResult=0)]
        [TestCase("WA 2000\r\nPSG1\r\n", CsvRecordTerminator.CRLF, ExpectedResult=7)]
        [TestCase("WA 2000\nPSG1\n", CsvRecordTerminator.LF, ExpectedResult=7)]
        [TestCase("\"\"\r\n\"NTW-20\"\r\n", CsvRecordTerminator.CRLF, ExpectedResult=2)]
        [TestCase("\"Noreen \"\"Bad News\"\" ULR 338\"\r\n\"NTW-20\"\r\n", CsvRecordTerminator.CRLF, ExpectedResult=29)]
        [TestCase("\"7.62mmx51mm NATO\r\n.300Winchester Magnum\r\n7.5x55mm Swiss\"\r\n\"9x19mm Parabellum\"\r\n", CsvRecordTerminator.CRLF, ExpectedResult=57)]
        public int Parse_LastFieldEndsWithRecordTerminator_ReturnsLengthToRecordTerminator(string str, CsvRecordTerminator recordTerminator)
        {
            var parser = CreateParser(recordTerminator: recordTerminator);
            var result = parser.Parse(str, 0, 1, 1);

            return result.Length;
        }

        [TestCase("", ExpectedResult=0)]
        [TestCase("WA 2000", ExpectedResult=7)]
        [TestCase("\"\"", ExpectedResult=2)]
        [TestCase("\"WA 2000\"", ExpectedResult=9)]
        public int Parse_LastFieldEndsWithEOF_ReturnsLengthToEOF(string str)
        {
            var parser = CreateParser();
            var result = parser.Parse(str, 0, 1, 1);

            return result.Length;
        }

        [TestCase("", ExpectedResult="")]
        [TestCase("Heckler & Koch,PSG1", ExpectedResult="Heckler & Koch")]
        public string Parse_UnquotedField_ReturnsValueAsItIs(string str)
        {
            var parser = CreateParser();
            var result = parser.Parse(str, 0, 1, 1);

            return result.Value;
        }

        [TestCase("Gas-operated\\,Rotating bolt,Semi-automatic", ',', '\\', ExpectedResult="Gas-operated,Rotating bolt")]
        [TestCase("Gas-operated$\tRotating bolt\tSemi-automatic", '\t', '$', ExpectedResult="Gas-operated\tRotating bolt")]
        public string Parse_UnquotedFieldHasFieldSeparatorEscapeSequences_ValueIncludeFieldSeparator(string str, char separator, char escape)
        {
            var parser = CreateParser(separator: separator, escape: escape);
            var result = parser.Parse(str, 0, 1, 1);

            return result.Value;
        }

        [TestCase("\\\r\n", '\\', CsvRecordTerminator.CR, ExpectedResult="\r\n")]
        [TestCase("\\\r\n", '\\', CsvRecordTerminator.CRLF, ExpectedResult="\r\n")]
        [TestCase("\r\\\n", '\\', CsvRecordTerminator.LF, ExpectedResult="\r\n")]
        [TestCase("7.62mmx51mm NATO\\\r\n.300Winchester Magnum\\\r\n7.5x55mm Swiss\r\n9x19mm Parabellum\r\n", '\\', CsvRecordTerminator.CRLF, ExpectedResult="7.62mmx51mm NATO\r\n.300Winchester Magnum\r\n7.5x55mm Swiss")]
        [TestCase("7.62mmx51mm NATO/\n.300Winchester Magnum/\n7.5x55mm Swiss\n9x19mm Parabellum\n", '/', CsvRecordTerminator.LF, ExpectedResult="7.62mmx51mm NATO\n.300Winchester Magnum\n7.5x55mm Swiss")]
        public string Parse_UnquotedFieldHasRecordTerminatorEscapeSequences_ValueIncludeRecordTerminator(string str, char escape, CsvRecordTerminator recordTerminator)
        {
            var parser = CreateParser(escape: escape, recordTerminator: recordTerminator);
            var result = parser.Parse(str, 0, 1, 1);

            return result.Value;
        }

        [TestCase("Noreen \"\"Bad News\"\" ULR 338\"", '"', 1, 28)]
        [TestCase("7.62mmx51mm NATO\\\r\n.300Winchester Magnum\\\r\n7.5x55mm Swiss\\", '\\', 3, 15)]
        public void Parse_UnquotedFieldEndsWithEscape_ThrowsException(string str, char escape, int lineNumber, int linePosition)
        {
            var parser = CreateParser(escape: escape);

            Assert.Throws(
                TypeOf<CsvParsingException>().And
                    .Property("LineNumber").EqualTo(lineNumber).And
                    .Property("LinePosition").EqualTo(linePosition),
                () => parser.Parse(str, 0, 1, 1));
        }

        [TestCase("\"\"", '"', ExpectedResult="")]
        [TestCase("\'\'", '\'', ExpectedResult="")]
        [TestCase("\"Heckler & Koch\",PSG1", '"', ExpectedResult="Heckler & Koch")]
        [TestCase("\'Heckler & Koch\',PSG1", '\'', ExpectedResult="Heckler & Koch")]
        public string Parse_QuotedField_ReturnsValueExceptQuote(string str, char quote)
        {
            var parser = CreateParser(quote: quote);
            var result = parser.Parse(str, 0, 1, 1);

            return result.Value;
        }

        [TestCase("\"Noreen \"\"Bad News\"\" ULR 338\"", '"', '"', ExpectedResult="Noreen \"Bad News\" ULR 338")]
        [TestCase("\'Noreen \\\'Bad News\\\' ULR 338\'", '\'', '\\', ExpectedResult="Noreen \'Bad News\' ULR 338")]
        public string Parse_QuotedFieldContainsEscapedQuote_RemovesEscape(string str, char quote, char escape)
        {
            var parser = CreateParser(quote: quote, escape: escape);
            var result = parser.Parse(str, 0, 1, 1);

            return result.Value;
        }

        [TestCase("\"Noreen \"\"Bad News\"\" ULR 338", '"')]
        [TestCase("\'Noreen \'\'Bad News\'\' ULR 338", '\'')]
        public void Parse_QuotedFieldIsNotClosed_ThrowsException(string str, char quote)
        {
            var parser = CreateParser(quote: quote, escape: quote);

            Assert.Throws(
                TypeOf<CsvParsingException>().And.Property("LinePosition").EqualTo(1),
                () => parser.Parse(str, 0, 1, 1));
        }

        [TestCase("\"Heckler & Koch\" ,PSG1", 17)]
        public void Parse_QuotedFieldEndsWithInvalidCharacter_ThrowsException(string str, int linePosition)
        {
            var parser = CreateParser();

            Assert.Throws(
                TypeOf<CsvParsingException>().And.Property("LinePosition").EqualTo(linePosition),
                () => parser.Parse(str, 0, 1, 1));
        }

        [TestCase("7.62mmx51mm NATO\\\r.300Winchester Magnum\\\r7.5x55mm Swiss", '\\', 346, ExpectedResult=348)]
        [TestCase("\"7.62mmx51mm NATO\r\n.300Winchester Magnum\r\n7.5x55mm Swiss\"", '"', 765, ExpectedResult=767)]
        public int Parse_FieldConsistsOfSingleOrMultipleLines_ReturnsRelativeLineNumberFromLineNumberParam(string str, char escape, int lineNumber)
        {
            var parser = CreateParser(escape: escape);
            var result = parser.Parse(str, 0, lineNumber, 1);

            return result.LineNumber;
        }

        [Test]
        public void Parse_ExceptionOccurred_LineNumberParamAppliedToLineNumberPropertyOfException()
        {
            var parser = CreateParser();
            var strEndsWithEscape = "7.62mmx51mm NATO\"\r\n.300Winchester Magnum\"\r\n7.5x55mm Swiss\"";
            var lineNumber = 765;
            var expectedLineNumber = 767;

            Assert.Throws(
                TypeOf<CsvParsingException>().And.Property("LineNumber").EqualTo(expectedLineNumber),
                () => parser.Parse(strEndsWithEscape, 0, lineNumber, 1));
        }

        [TestCase("Heckler & Koch", 346, ExpectedResult=360)]
        [TestCase("\"Accuracy International\"", 765, ExpectedResult=789)]
        public int Parse_FieldConsistsOfSingleLine_ReturnsRelativeLinePositionFromLinePositionParam(string str, int linePosition)
        {
            var parser = CreateParser();
            var result = parser.Parse(str, 0, 1, linePosition);

            return result.LinePosition;
        }

        [TestCase("7.62mmx51mm NATO\n.300Winchester Magnum\n7.5x55mm Swiss", '\\', 765, ExpectedResult=15)]
        [TestCase("\"7.62mmx51mm NATO\r\n.300Winchester Magnum\r\n7.5x55mm Swiss\"", '"', 765, ExpectedResult=16)]
        public int Parse_FieldConsistsOfMultipleLines_LinePositionParamNotAffectsToResultLinePosition(string str, char escape, int linePosition)
        {
            var parser = CreateParser(escape: escape);
            var result = parser.Parse(str, 0, 1, linePosition);

            return result.LinePosition;
        }

        [Test]
        public void Parse_ExceptionOccurred_LinePositionParamAppliedToLinePositionPropertyOfException()
        {
            var parser = CreateParser();
            var strEndsWithEscape = "9x19mm Parabellum\"";
            var linePosition = 765;
            var expectedLinePosition = 782;

            Assert.Throws(
                TypeOf<CsvParsingException>().And.Property("LinePosition").EqualTo(expectedLinePosition),
                () => parser.Parse(strEndsWithEscape, 0, 1, linePosition));
        }

        [TestCase("Heckler & Koch,PSG1", 10, ExpectedResult="Koch")]
        [TestCase("\"Heckler & Koch\",\"PSG1\"", 17, ExpectedResult="PSG1")]
        public string Parse_StartIndexParamIsValid_UsesForFieldStartPosition(string str, int startIndex)
        {
            var parser = CreateParser();
            var result = parser.Parse(str, startIndex, 1, 1);

            return result.Value;
        }

        [TestCase(CsvTrimMode.Left, " Heckler & Koch ", ExpectedResult="Heckler & Koch ")]
        [TestCase(CsvTrimMode.Right, " Heckler & Koch ", ExpectedResult=" Heckler & Koch")]
        [TestCase(CsvTrimMode.Both, " Heckler & Koch ", ExpectedResult="Heckler & Koch")]
        public string Parse_TrimModeParamOfCtorIsNotNone_ReturnsTrimmedValue(CsvTrimMode trimMode, string str)
        {
            var parser = CreateParser(trimMode: trimMode);
            var result = parser.Parse(str, 0, 1, 1);

            return result.Value;
        }

        [Test]
        public void Parse_QuotedField_NotAffectedTrimMode()
        {
            var parser = CreateParser(trimMode: CsvTrimMode.Both);
            var result = parser.Parse("\" Heckler & Koch \",PSG1", 0, 1, 1);

            Assert.That(result.Value, EqualTo(" Heckler & Koch "));
        }

        [TestCase("", '"')]
        [TestCase("\\N", '\\')]
        [TestCase("NULL", '"')]
        public void Parse_NullValueParamOfCtorIsNotNull_ReturnsNullValueIfMatched(string str, char escape)
        {
            var parser = CreateParser(escape: escape, nullValue: str);
            var result = parser.Parse(str, 0, 1, 1);

            Assert.That(result.Value, Null);
        }

        private StringFieldParser CreateParser(
            char separator = ',',
            char? quote = '"',
            char? escape = '"',
            CsvRecordTerminator recordTerminator = CsvRecordTerminator.CRLF,
            CsvTrimMode trimMode = CsvTrimMode.None,
            string nullValue = null)
        {
            return new StringFieldParser(separator, quote, escape, recordTerminator, trimMode, nullValue);
        }
    }
}
