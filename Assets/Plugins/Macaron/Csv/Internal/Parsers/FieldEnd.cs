#if UNITY_EDITOR
[assembly:System.Runtime.CompilerServices.InternalsVisibleTo("Assembly-CSharp-Editor")]
#endif
namespace Macaron.Csv.Internal.Parsers
{
    internal enum FieldEnd
    {
        EOF,
        Separator,
        CR,
        LF,
        CRLF
    }
}
