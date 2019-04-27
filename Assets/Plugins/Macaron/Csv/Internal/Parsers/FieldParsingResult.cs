#if UNITY_EDITOR
[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("Assembly-CSharp-Editor")]
#endif
namespace Macaron.Csv.Internal.Parsers
{
    internal struct FieldParsingResult
    {
        public string Value;
        public int Length;
        public FieldEnd End;
        public bool IsLast;
        public int LineNumber;
        public int LinePosition;
    }
}
