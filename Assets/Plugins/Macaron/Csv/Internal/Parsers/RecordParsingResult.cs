#if UNITY_EDITOR
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Assembly-CSharp-Editor")]
#endif
namespace Macaron.Csv.Internal.Parsers
{
    internal struct RecordParsingResult
    {
        public string[] Values;
        public CsvRecordTerminator? Terminator;
        public int Length;
        public int LineNumber;
        public int LinePosition;
    }
}
