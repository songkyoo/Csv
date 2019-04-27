using System;

namespace Macaron.Csv
{
    partial struct CsvReaderSettings
    {
        public static readonly CsvReaderSettings Default;
        public static readonly CsvReaderSettings RFC4810;
        public static readonly CsvReaderSettings Excel;
        public static readonly CsvReaderSettings MySQL;

        static CsvReaderSettings()
        {
            Default = new CsvReaderSettings
            {
                FieldSeparator = ',',
                Quote = '"',
                Escape = '"'
            };
            RFC4810 = new CsvReaderSettings
            {
                FieldSeparator = ',',
                Quote = '"',
                Escape = '"',
                RecordTerminator = CsvRecordTerminator.CRLF
            };
            Excel = new CsvReaderSettings
            {
                FieldSeparator = ',',
                Quote = '"',
                Escape = '"',
                RecordTerminator = CsvRecordTerminator.CRLF
            };
            MySQL = new CsvReaderSettings
            {
                FieldSeparator = '\t',
                Quote = null,
                Escape = '\\',
                RecordTerminator = CsvRecordTerminator.LF
            };
        }
    }
}