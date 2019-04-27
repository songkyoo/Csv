using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Macaron.Csv;
using Macaron.Csv.Iterators;

namespace Macaron.Tests.Csv.Iterators
{
    [TestFixture]
    public abstract class CsvIteratorTest : AssertionHelper
    {
        [Test]
        public void Record_MoveNextMethodNotCalled_ReturnsNull()
        {
            var iterator = CreateIterator("Heckler & Koch,PSG1\r\nAccuracy International,AWP\r\n");

            Assert.That(iterator.Record, Null);
        }

        [Test]
        public void MoveNext_EmptyString_ReturnsFalse()
        {
            var iterator = CreateIterator(string.Empty);
            var result = iterator.MoveNext();

            Assert.That(result, False);
        }

        [TestCase("Heckler & Koch,PSG1\r\nAccuracy International,AWP\r\n")]
        [TestCase("PSG1,7.62x51mm NATO\r\nAWP,\".243 Winchester\r\n7.62x51mm NATO\"\r\nNTW-20,20x82mm")]
        public void MoveNext_ReturnsFalse_RecordPropertyRerturnsNull(string str)
        {
            var iterator = CreateIterator(str);

            while (iterator.MoveNext())
            {
                // 아무것도 하지 않는다.
            }

            Assert.That(iterator.Record, Null);
        }

        [TestCase(
            "Denel,NTW-20,Bolt action\r\nWalther,WA 2000,\"Gas-operated,Rotating bolt\"\r\n",
            ExpectedResult="Denel,NTW-20,Bolt action,Walther,WA 2000,Gas-operated,Rotating bolt")]
        [TestCase(
            "WA 2000,\"7.62x51mm\n.300 Winchester Magnum\n7.5x55mm Swiss\"\nNTW-20,20x82mm\n",
            ExpectedResult="WA 2000,7.62x51mm\n.300 Winchester Magnum\n7.5x55mm Swiss,NTW-20,20x82mm")]
        public string MoveNext_ReturnsTrue_RecordPropertyRerturnsRecordValue(string str)
        {
            var iterator = CreateIterator(str);
            var results = new List<string>();

            while (iterator.MoveNext())
            {
                results.AddRange(iterator.Record);
            }

            return string.Join(",", results.ToArray());
        }

        [TestCase("\r\n")]
        [TestCase("AWP,\".243 Winchester\r\n7.62x51mm NATO\"")]
        public void MoveNext_ReturnsTrue_IncreasesRecordNumber(string str)
        {
            var iterator = CreateIterator(str);
            var recordNumber = 0;

            while (iterator.MoveNext())
            {
                recordNumber += 1;
            }

            Assert.That(iterator.RecordNumber, EqualTo(recordNumber));
        }

        [TestCase("Walther,WA 2000,7.62x51mm\r\\\n.300 Winchester Magnum\r\\\n7.5x55mm Swiss\n", '\\', 15)]
        public void MoveNext_RecordTerminatorIsNullAndUnquotedFieldHasNewline_ThrowsException(string str, char escape, int linePosition)
        {
            var iterator = CreateIterator(str, escape: escape, recordTerminator: null);

            Assert.Throws(
                TypeOf<CsvParsingException>().And.Property("LinePosition").EqualTo(linePosition),
                () =>
                {
                    while (iterator.MoveNext())
                    {
                        // 아무것도 하지 않는다.
                    }
                });
        }

        [TestCase("Denel,NTW-20,Bolt action", 1)]
        [TestCase("Denel,NTW-20,Bolt action\r\nWalther,WA 2000,\"Gas-operated,Rotating bolt\"\r\n", 2)]
        public void MoveNext_MultipleRecords_ReturnsTrueAsMuchAsRecordCount(string str, int recordCount)
        {
            var iterator = CreateIterator(str);
            var results = new List<bool>();

            // true
            for (int i = 0; i < recordCount; ++i)
            {
                results.Add(iterator.MoveNext());
            }

            // false
            results.Add(iterator.MoveNext());

            Assert.That(results, EqualTo(Enumerable.Repeat(true, recordCount).Concat(new [] { false })));
        }

        [TestCase("Denel,NTW-20,Bolt action\r\nWalther,WA 2000,\"Gas-operated,Rotating bolt\"\r\n", ExpectedResult=new[] { "1,25", "2,45" })]
        public string[] MoveNext_MultipleRecords_LinePositionEqualsToEndOfRecordWithoutRecordTerminator(string str)
        {
            var iterator = CreateIterator(str);
            var results = new List<string>();

            while (iterator.MoveNext())
            {
                results.Add(iterator.LineNumber + "," + iterator.LinePosition);
            }

            return results.ToArray();
        }

        [Test]
        public void MoveNext_DisposeMethodCalled_ThrowsException()
        {
            var iterator = CreateIterator("");
            iterator.Dispose();

            Assert.Throws(TypeOf<ObjectDisposedException>(), () => iterator.MoveNext());
        }

        protected abstract ICsvIterator CreateIterator(
            string str,
            char fieldSeparator = ',',
            char? quote = '"',
            char? escape = '"',
            CsvRecordTerminator? recordTerminator = null,
            CsvTrimMode trimMode = CsvTrimMode.None,
            string nullValue = null);
    }
}
