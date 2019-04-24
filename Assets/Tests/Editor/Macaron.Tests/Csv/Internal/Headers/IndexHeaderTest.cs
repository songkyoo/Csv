using System;
using NUnit.Framework;
using Macaron.Csv.Internal.Headers;

namespace Macaron.Tests.Csv.Internal.Headers
{
    [TestFixture]
    public class IndexHeaderTest : AssertionHelper
    {
        [Test]
        public void Ctor_CountParamIsLessThanZero_ThrowsException()
        {
            Assert.Throws(
                TypeOf<ArgumentOutOfRangeException>().And.Property("ParamName").EqualTo("count"),
                () => new IndexHeader(-1));
        }

        [Test]
        public void Item_SetValue_ThrowsException()
        {
            var header = new IndexHeader(1);

            Assert.Throws(TypeOf<NotSupportedException>(), () => header[0] = "Denel");
        }

        [Test]
        public void GetIndex_ColumnNameParamIsInRange_ReturnsColumnNameValue()
        {
            var header = new IndexHeader(3);

            Assert.That(header.GetIndex(1), EqualTo(1));
        }

        [TestCase(0, -1)]
        [TestCase(1, 1)]
        public void GetIndex_ColumnNameParamIsOutOfRange_ReturnsMinusOne(int count, int columnName)
        {
            var header = new IndexHeader(count);

            Assert.That(header.GetIndex(columnName), EqualTo(-1));
        }
    }
}
