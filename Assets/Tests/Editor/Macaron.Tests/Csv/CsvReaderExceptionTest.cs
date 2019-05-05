using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using Macaron.Csv;

namespace Macaron.Tests.Csv
{
    [TestFixture]
    public class CsvReaderExceptionTest : AssertionHelper
    {
        [Test]
        public void Ctor_UsesSerializedStream_HasSameValueAsOriginalInstance()
        {
            var message = "Message.";
            var recordNumber = int.MaxValue;
            var columnName = "ColumnName";

            var exception = new CsvReaderException(message, null, recordNumber, columnName);

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, exception);
                stream.Position = 0;
                exception = (CsvReaderException)formatter.Deserialize(stream);
            }

            Assert.That(exception.Message, StartWith(message));
            Assert.That(exception.RecordNumber, EqualTo(recordNumber));
            Assert.That(exception.ColumnName, EqualTo(columnName));
        }
    }
}
