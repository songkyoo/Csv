using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Macaron.Csv;
using Macaron.Csv.Iterators;

namespace Macaron.Tests.Csv.Iterators
{
    [TestFixture]
    public class CsvStringIteratorTest : CsvIteratorTest
    {
        [Test]
        public void Ctor_StrParamIsNull_ThrowsException()
        {
            Assert.Throws(
                TypeOf<ArgumentNullException>().And.Property("ParamName").EqualTo("str"),
                () => new CsvStringIterator(null));
        }

        protected override ICsvIterator CreateIterator(
            string str,
            char fieldSeparator,
            char? quote,
            char? escape,
            CsvRecordTerminator? recordTerminator,
            CsvTrimMode trimMode,
            string nullValue)
        {
            return new CsvStringIterator(str, fieldSeparator, quote, escape, recordTerminator, trimMode, nullValue);
        }
    }
}
