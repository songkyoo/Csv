using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using Macaron.Csv;

namespace Macaron.Tests.Csv
{
    [TestFixture]
    public class CsvParsingExceptionTest : AssertionHelper
    {
        [Test]
        public void Ctor_UsesSerializedStream_HasSameValueAsOriginalInstance()
        {
            var message = "Message.";
            var lineNumber = int.MinValue;
            var linePosition = int.MaxValue;

            var exception = new CsvParsingException(message, null, lineNumber, linePosition);

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, exception);
                stream.Position = 0;
                exception = (CsvParsingException)formatter.Deserialize(stream);
            }

            Assert.That(exception.Message, StartWith(message));
            Assert.That(exception.LineNumber, EqualTo(lineNumber));
            Assert.That(exception.LinePosition, EqualTo(linePosition));
        }
    }
}
