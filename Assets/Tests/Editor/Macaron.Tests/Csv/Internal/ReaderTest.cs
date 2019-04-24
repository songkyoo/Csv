using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Macaron.Csv;
using Macaron.Csv.Internal;
using Macaron.Csv.Internal.HeaderPolicies;
using Macaron.Csv.Iterators;

namespace Macaron.Tests.Csv.Internal
{
    [TestFixture]
    public class ReaderTest : AssertionHelper
    {
        [Test]
        public void Read_EmptyString_ReturnsFalse()
        {
            AssertNumberOfRecords(string.Empty, 0);
        }

        [TestCase("\r\n")]
        [TestCase("M1911\r\n")]
        [TestCase("M1911")]
        public void Read_SingleRecord_ReturnsTrueThenFalse(string input)
        {
            AssertNumberOfRecords(input, 1);
        }

        [TestCase("\r\n\r\n\r\n")]
        [TestCase("M1911\r\nAWP\r\nNTW-20\r\n")]
        [TestCase("M1911\r\nAWP\r\nNTW-20")]
        public void Read_MultipleRecords_ReturnsTrueAsMuchAsNumberOfRecords(string input)
        {
            AssertNumberOfRecords(input, 3);
        }

        [TestCase("\r\n\"\r\n\"\r\n\r\n")]
        [TestCase(".45 ACP\r\n\".243 Winchester\r\n7.62x51mm NATO\"\r\n20x82mm\r\n")]
        [TestCase(".45 ACP\r\n\".243 Winchester\r\n7.62x51mm NATO\"\r\n20x82mm")]
        public void Read_MultipleRecords_ReturnsTrueAsMuchAsNumberOfRecordsNotLine(string input)
        {
            AssertNumberOfRecords(input, 3);
        }

        [Test]
        public void Read_DisposeMethodsCalled_ThrowsException()
        {
            var reader = CreateReader("");
            reader.Dispose();

            Assert.Throws(TypeOf<ObjectDisposedException>(), () => reader.Read());
        }

        [Test]
        public void Read_NumberOfFieldsIsNotEqual_ThrowsException()
        {
            var reader = CreateReader("Manufacturer,Name,Cartridge\r\nHeckler & Koch,PSG1\r\nDenel,NTW-20,20x82mm\r\n");

            Assert.Throws(
                TypeOf<CsvReaderException>().And.Property("RecordNumber").EqualTo(2),
                () => { while (reader.Read()); });
        }

        [Test]
        public void Record_ReadMethodNotCalled_ReturnsNull()
        {
            var reader = CreateReader("Heckler & Koch,PSG1,76x51mm NATO\r\nDenel,NTW-20,20x82mm\r\n");

            Assert.That(reader.Record, Null);
        }

        [TestCase(
            "Heckler & Koch,PSG1,76x51mm NATO\r\nDenel,NTW-20,20x82mm\r\n",
            ExpectedResult="Heckler & Koch:PSG1:76x51mm NATO:Denel:NTW-20:20x82mm")]
        public string Record_ReadMethodReturnsTrue_ReturnsRecord(string str)
        {
            var reader = CreateReader(str);
            var result = new List<string>();

            while (reader.Read())
            {
                result.AddRange(reader.Record);
            }

            return string.Join(":", result.ToArray());
        }

        [Test]
        public void Record_MoveNextMethodReturnsFalse_ReturnsNull()
        {
            var reader = CreateReader("Heckler & Koch,PSG1,76x51mm NATO\r\nDenel,NTW-20,20x82mm\r\n");

            while (reader.Read())
            {
                // 아무것도 하지 않는다.
            }

            Assert.That(reader.Record, Null);
        }

        private void AssertNumberOfRecords(string str, int recordCount)
        {
            var reader = CreateReader(str);
            var results = new List<bool>();

            // true
            for (int i = 0; i < recordCount; ++i)
            {
                results.Add(reader.Read());
            }

            // false
            results.Add(reader.Read());

            Assert.That(results, EqualTo(Enumerable.Repeat(true, recordCount).Concat(new [] { false })));
        }

        private ICsvReader<int> CreateReader(
            string str,
            char fieldSeparator = ',',
            char? quote = '"',
            char? escape = '"',
            CsvRecordTerminator? recordTerminator = null,
            CsvTrimMode trimMode = CsvTrimMode.None,
            string nullValue = null)
        {
            var iterator = new CsvStringIterator(
                str,
                fieldSeparator,
                quote,
                escape,
                recordTerminator,
                trimMode,
                nullValue);

            return new Reader<int>(iterator, Index.Instance, true);
        }
    }
}
