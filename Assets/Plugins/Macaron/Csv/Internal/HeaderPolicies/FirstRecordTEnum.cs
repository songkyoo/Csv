using System;
using System.Linq;
using Macaron.Csv.Internal.Headers;

namespace Macaron.Csv.Internal.HeaderPolicies
{
    internal class FirstRecord<TEnum> : ICsvHeaderPolicy<TEnum> where TEnum : struct, IConvertible
    {
        private readonly Func<string, string> _selector;

        public FirstRecord(Func<string, string> selector)
        {
            // EnumHeader의 생성자에서도 유효성을 확인하지만 헤더 생성 시점이 아닌 헤더 정책 생성 시점에서 오류를 알 수
            // 있도록 하기 위해서 오류 메세지를 확인한다.
            var typeErrorMessage = EnumHeader<TEnum>.TypeErrorMessage;
            if (typeErrorMessage.Length > 0)
            {
                throw new ArgumentException(typeErrorMessage, "TEnum");
            }

            _selector = selector;
        }

        public ICsvHeader<TEnum> CreateHeader(ICsvIterator iterator)
        {
            if (iterator == null)
            {
                throw new ArgumentNullException("iterator");
            }

            var record = iterator.Record;

            if (record == null)
            {
                throw new ArgumentException("Record 속성이 null이 아니어야 합니다.", "iterator");
            }

            var columnNames = _selector != null ? record.Select(_selector).ToArray() : record;
            iterator.MoveNext();

            return new EnumHeader<TEnum>(columnNames);
        }
    }
}
