using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Macaron.Csv;
using Macaron.Csv.Iterators;

namespace Macaron.Tests.Csv.Iterators
{
    [TestFixture]
    public class CsvStreamIteratorTest : CsvIteratorTest
    {
        [Test]
        public void Ctor_StreamParamIsNull_ThrowsException()
        {
            Assert.Throws(
                TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("stream"),
                () => new CsvStreamIterator((Stream)null));
        }

        [Test]
        public void Ctor_ReaderParamIsNull_ThrowsException()
        {
            Assert.Throws(
                TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("reader"),
                () => new CsvStreamIterator((TextReader)null));
        }

        [Test]
        public void Dispose_LeaveOpenParamOfCtorIsFalse_DisposeStream()
        {
            var stream = GetStream(string.Empty);
            var iterator = new CsvStreamIterator(stream, leaveOpen: false);

            iterator.Dispose();

            Assert.Throws(TypeOf<ObjectDisposedException>(), () => { var pos = stream.Position; });
        }

        [Test]
        public void Dispose_LeaveOpenParamOfCtorIsTrue_DoesNotDisposeStream()
        {
            var stream = GetStream(string.Empty);
            var iterator = new CsvStreamIterator(stream, leaveOpen: true);

            iterator.Dispose();

            Assert.DoesNotThrow(() => { var pos = stream.Position; });
        }

        [Test]
        public void Dispose_LeaveOpenParamOfCtorIsFalse_DisposeTextReader()
        {
            var stream = GetStream(string.Empty);
            var reader = new StreamReader(stream);
            var iterator = new CsvStreamIterator(reader, leaveOpen: false);

            iterator.Dispose();

            Assert.Throws(TypeOf<ObjectDisposedException>(), () => { var eos = reader.EndOfStream; });
        }

        [Test]
        public void Dispose_LeaveOpenParamOfCtorIsTrue_DoesNotDisposeTextReader()
        {
            var stream = GetStream(string.Empty);
            var reader = new StreamReader(stream);
            var iterator = new CsvStreamIterator(reader, leaveOpen: true);

            iterator.Dispose();

            Assert.DoesNotThrow(() => { var eos = reader.EndOfStream; });
        }

        protected override ICsvIterator CreateIterator(
            string str,
            char fieldSeparator = ',',
            char? quote = '"',
            char? escape = '"',
            CsvRecordTerminator? recordTerminator = null,
            CsvTrimMode trimMode = CsvTrimMode.None,
            string nullValue = null)
        {
            var stream = GetStream(str);
            return new CsvStreamIterator(
                stream,
                false,
                fieldSeparator,
                quote,
                escape,
                recordTerminator,
                trimMode,
                nullValue);
        }

        private Stream GetStream(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            return new MemoryStream(bytes);
        }
    }
}
