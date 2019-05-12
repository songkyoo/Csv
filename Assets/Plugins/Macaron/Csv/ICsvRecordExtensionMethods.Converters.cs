using System;
using System.Globalization;
using UnityEngine;

namespace Macaron.Csv
{
    partial class ICsvRecordExtensionMethod
    {
        private interface IConverter<T>
        {
            T Convert(string value);
        }

        private struct NullableBooleanConverter : IConverter<bool?>
        {
            public bool? Convert(string value)
            {
                return bool.Parse(value);
            }
        }

        private struct NullableByteConverter : IConverter<byte?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableByteConverter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public byte? Convert(string value)
            {
                return byte.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableCharConverter : IConverter<char?>
        {
            public char? Convert(string value)
            {
                return char.Parse(value);
            }
        }

        private struct NullableDecimalConverter : IConverter<decimal?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableDecimalConverter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public decimal? Convert(string value)
            {
                return decimal.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableDoubleConverter : IConverter<double?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableDoubleConverter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public double? Convert(string value)
            {
                return double.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableInt16Converter : IConverter<short?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableInt16Converter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public short? Convert(string value)
            {
                return short.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableInt32Converter : IConverter<int?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableInt32Converter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public int? Convert(string value)
            {
                return int.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableInt64Converter : IConverter<long?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableInt64Converter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public long? Convert(string value)
            {
                return long.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableSByteConverter : IConverter<sbyte?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableSByteConverter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public sbyte? Convert(string value)
            {
                return sbyte.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableSingleConverter : IConverter<float?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableSingleConverter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public float? Convert(string value)
            {
                return float.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableUInt16Converter : IConverter<ushort?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableUInt16Converter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public ushort? Convert(string value)
            {
                return ushort.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableUInt32Converter : IConverter<uint?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableUInt32Converter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public uint? Convert(string value)
            {
                return uint.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableUInt64Converter : IConverter<ulong?>
        {
            private NumberStyles _styles;
            private IFormatProvider _provider;

            public NullableUInt64Converter(NumberStyles styles, IFormatProvider provider)
            {
                _styles = styles;
                _provider = provider;
            }

            public ulong? Convert(string value)
            {
                return ulong.Parse(value, _styles, _provider ?? NumberFormatInfo.CurrentInfo);
            }
        }

        private struct NullableEnumConverter<T> : IConverter<T?> where T : struct
        {
            private bool _ignoreCase;

            public NullableEnumConverter(bool ignoreCase)
            {
                _ignoreCase = ignoreCase;
            }

            public T? Convert(string value)
            {
                return (T)Enum.Parse(typeof(T), value, _ignoreCase);
            }
        }

        private struct NullableDateTimeConverter : IConverter<DateTime?>
        {
            private string _format;
            private string[] _formats;
            private IFormatProvider _provider;
            private DateTimeStyles _styles;

            public NullableDateTimeConverter(
                string format,
                string[] formats,
                IFormatProvider provider,
                DateTimeStyles styles)
            {
                _format = format;
                _formats = formats;
                _provider = provider;
                _styles = styles;
            }

            public DateTime? Convert(string value)
            {
                var provider = _provider ?? DateTimeFormatInfo.CurrentInfo;

                if (!string.IsNullOrEmpty(_format))
                {
                    return DateTime.ParseExact(value, _format, provider, _styles);
                }
                else if (_formats != null)
                {
                    return DateTime.ParseExact(value, _formats, provider, _styles);
                }
                else
                {
                    return DateTime.Parse(value, provider, _styles);
                }
            }
        }

#if !UNITY_5_6_OR_NEWER || NET_2_0 || NET_2_0_SUBSET
        private struct NullableTimeSpanConverter : IConverter<TimeSpan?>
        {
            public TimeSpan? Convert(string value)
            {
                return TimeSpan.Parse(value);
            }
        }
#else
        private struct NullableTimeSpanConverter : IConverter<TimeSpan?>
        {
            private string _format;
            private string[] _formats;
            private IFormatProvider _provider;
            private TimeSpanStyles _styles;

            public NullableTimeSpanConverter(
                string format,
                string[] formats,
                IFormatProvider provider,
                TimeSpanStyles styles)
            {
                _format = format;
                _formats = formats;
                _provider = provider;
                _styles = styles;
            }

            public TimeSpan? Convert(string value)
            {
                var provider = _provider ?? DateTimeFormatInfo.CurrentInfo;

                if (!string.IsNullOrEmpty(_format))
                {
                    return TimeSpan.ParseExact(value, _format, provider, _styles);
                }
                else if (_formats != null)
                {
                    return TimeSpan.ParseExact(value, _formats, provider, _styles);
                }
                else
                {
                    return TimeSpan.Parse(value, provider);
                }
            }
        }
#endif

        private struct NullableDateTimeOffsetConverter : IConverter<DateTimeOffset?>
        {
            private string _format;
            private string[] _formats;
            private IFormatProvider _provider;
            private DateTimeStyles _styles;

            public NullableDateTimeOffsetConverter(
                string format,
                string[] formats,
                IFormatProvider provider,
                DateTimeStyles styles)
            {
                _format = format;
                _formats = formats;
                _provider = provider;
                _styles = styles;
            }

            public DateTimeOffset? Convert(string value)
            {
                var provider = _provider ?? DateTimeFormatInfo.CurrentInfo;

                if (!string.IsNullOrEmpty(_format))
                {
                    return DateTimeOffset.ParseExact(value, _format, provider, _styles);
                }
                else if (_formats != null)
                {
                    return DateTimeOffset.ParseExact(value, _formats, provider, _styles);
                }
                else
                {
                    return DateTimeOffset.Parse(value, provider, _styles);
                }
            }
        }

#if !UNITY_5_6_OR_NEWER || NET_2_0 || NET_2_0_SUBSET
        private struct NullableGuidConverter : IConverter<Guid?>
        {
            public Guid? Convert(string value)
            {
                return new Guid(value);
            }
        }
#else
        private struct NullableGuidConverter : IConverter<Guid?>
        {
            private string _format;

            public NullableGuidConverter(string format)
            {
                _format = format;
            }

            public Guid? Convert(string value)
            {
                if (!string.IsNullOrEmpty(_format))
                {
                    return Guid.ParseExact(value, _format);
                }
                else
                {
                    return Guid.Parse(value);
                }
            }
        }
#endif

        private struct UriConverter : IConverter<Uri>
        {
            private UriKind _uriKind;

            public UriConverter(UriKind uriKind)
            {
                _uriKind = uriKind;
            }

            public Uri Convert(string value)
            {
                return new Uri(value, _uriKind);
            }
        }

        private struct NullableColor32Converter : IConverter<Color32?>
        {
            public Color32? Convert(string value)
            {
                if (value[0] != '#')
                {
                    throw new FormatException();
                }

                byte r, g, b, a;
                ColorHelper.GetBytes(value, out r, out g, out b, out a);

                return new Color32(r, g, b, a);
            }
        }

        private struct NullableColorConverter : IConverter<Color?>
        {
            public Color? Convert(string value)
            {
                if (value[0] != '#')
                {
                    throw new FormatException();
                }

                byte r, g, b, a;
                ColorHelper.GetBytes(value, out r, out g, out b, out a);

                return new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
            }
        }
    }
}
