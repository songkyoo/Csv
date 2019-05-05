namespace Macaron.Csv.Internal.Headers
{
    partial class IndexHeader
    {
        private class ColumnNameGenerator
        {
            private char[] _chars = new char[0];

            public string Next()
            {
                Advance(_chars.Length - 1);
                return new string(_chars);
            }

            private void Advance(int index)
            {
                if (index < 0)
                {
                    var chars = new char[_chars.Length + 1];

                    for (int i = 1; i < chars.Length; ++i)
                    {
                        chars[i] = _chars[i - 1];
                    }

                    chars[0] = 'A';
                    _chars = chars;

                    return;
                }

                _chars[index] = (char)(_chars[index] + 1);

                if (_chars[index] < '\u005B')
                {
                    return;
                }
                else
                {
                    _chars[index] = 'A';
                    Advance(index - 1);
                }
            }
        }
    }
}
