using System;

namespace Macaron.Csv
{
    partial class ICsvRecordExtensionMethod
    {
        private static class ColorHelper
        {
            public static void GetBytes(string colorCode, out byte r, out byte g, out byte b, out byte a)
            {
                switch (colorCode.Length)
                {
                    case 9:
                        r = ColorHelper.ToByte(colorCode[1], colorCode[2]);
                        g = ColorHelper.ToByte(colorCode[3], colorCode[4]);
                        b = ColorHelper.ToByte(colorCode[5], colorCode[6]);
                        a = ColorHelper.ToByte(colorCode[7], colorCode[8]);
                        break;

                    case 7:
                        r = ColorHelper.ToByte(colorCode[1], colorCode[2]);
                        g = ColorHelper.ToByte(colorCode[3], colorCode[4]);
                        b = ColorHelper.ToByte(colorCode[5], colorCode[6]);
                        a = 255;
                        break;

                    case 5:
                        r = ColorHelper.ToByte(colorCode[1], colorCode[1]);
                        g = ColorHelper.ToByte(colorCode[2], colorCode[2]);
                        b = ColorHelper.ToByte(colorCode[3], colorCode[3]);
                        a = ColorHelper.ToByte(colorCode[4], colorCode[4]);
                        break;

                    case 4:
                        r = ColorHelper.ToByte(colorCode[1], colorCode[1]);
                        g = ColorHelper.ToByte(colorCode[2], colorCode[2]);
                        b = ColorHelper.ToByte(colorCode[3], colorCode[3]);
                        a = 255;
                        break;

                    default:
                        throw new FormatException();
                }
            }

            private static byte ToByte(char c1, char c2)
            {
                int b1 = GetValue(c1);
                int b2 = GetValue(c2);

                return (byte)((b1 << 4) | b2);
            }

            private static int GetValue(char c)
            {
                if (c >= 'a' && c <= 'f')
                {
                    return (byte)(c - '\u0057');
                }
                else if (c >= 'A' && c <= 'F')
                {
                    return (byte)(c - '\u0037');
                }
                else if (c >= '0' && c <= '9')
                {
                    return (byte)(c - '0');
                }

                throw new FormatException();
            }
        }
    }
}
