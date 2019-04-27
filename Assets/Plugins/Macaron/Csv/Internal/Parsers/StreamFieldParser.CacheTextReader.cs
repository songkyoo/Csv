using System.IO;

namespace Macaron.Csv.Internal.Parsers
{
    partial class StreamFieldParser
    {
        private struct CacheTextReader
        {
            private TextReader _reader;
            private int _current;
            private int _length;

            public CacheTextReader(TextReader reader)
            {
                _reader = reader;
                _current = -1;
                _length = 0;
            }

            public int Current
            {
                get { return _current; }
            }

            public int Next
            {
                get { return _reader.Peek(); }
            }

            public int Length
            {
                get { return _length;}
            }

            public int Read()
            {
                _current = _reader.Read();

                if (_current != -1)
                {
                    _length += 1;
                }

                return _current;
            }
        }
    }
}
