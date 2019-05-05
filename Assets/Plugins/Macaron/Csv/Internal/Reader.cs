using System;
using System.Collections.Generic;

#if UNITY_EDITOR
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Assembly-CSharp-Editor")]
#endif
namespace Macaron.Csv.Internal
{
    internal class Reader<T> : ICsvReader<T>
    {
        #region Fields
        private ICsvHeader<T> _header;
        private ICsvIterator _iterator;
        private ICsvRecord<T> _record;
        #endregion

        #region Constructors
        public Reader(ICsvIterator iterator, ICsvHeaderPolicy<T> headerPolicy)
        {
            if (iterator == null)
            {
                throw new ArgumentNullException("iterator");
            }

            if (headerPolicy == null)
            {
                throw new ArgumentNullException("headerPolicy");
            }

            iterator.MoveNext();

            if (iterator.Record != null)
            {
                _header = headerPolicy.CreateHeader(iterator);

                if (_header == null)
                {
                    throw new CsvException("ICsvHeaderPolicy<T>.CreateHeader 메서드는 null을 반환해서는 안 됩니다.");
                }
            }

            _iterator = iterator;
        }
        #endregion

        #region Implementation of ICsvReader<T>
        public IList<string> Header
        {
            get
            {
                ThrowIfDisposed();
                return _header;
            }
        }

        public ICsvRecord<T> Record
        {
            get
            {
                ThrowIfDisposed();
                return _record;
            }
        }

        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_iterator == null)
            {
                return;
            }

            _iterator.Dispose();

            _iterator = null;
            _header = null;
            _record = null;
        }

        public bool Read()
        {
            ThrowIfDisposed();

            var record = _iterator.Record;

            if (record == null)
            {
                _record = null;
                return false;
            }

            if (record.Length != _header.Count)
            {
                throw new CsvReaderException("필드 수가 일치하지 않습니다.", null, _iterator.RecordNumber, null);
            }

            _record = new Record<T>(_header, _iterator.RecordNumber, record);
            _iterator.MoveNext();

            return true;
        }
        #endregion

        private void ThrowIfDisposed()
        {
            if (_iterator == null)
            {
                throw new ObjectDisposedException(null);
            }
        }
    }
}
