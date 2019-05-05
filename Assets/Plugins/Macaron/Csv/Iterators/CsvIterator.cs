using System;
using System.Collections;

namespace Macaron.Csv.Iterators
{
    public abstract class CsvIterator : ICsvIterator
    {
        #region Fields
        private CsvRecordTerminator? _recordTerminator;
        private int _recordNumber;
        private bool _disposed;
        #endregion

        #region Overrides
        ~CsvIterator()
        {
            Dispose(false);
        }
        #endregion

        #region Implementation of ICsvIterator
        public string[] Record
        {
            get
            {
                ThrowIfDisposed();
                return GetRecord();
            }
        }

        public int RecordNumber
        {
            get
            {
                ThrowIfDisposed();
                return _recordNumber;
            }
        }

        public int LineNumber
        {
            get
            {
                ThrowIfDisposed();
                return GetLineNumber();
            }
        }

        public int LinePosition
        {
            get
            {
                ThrowIfDisposed();
                return GetLinePosition();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool MoveNext()
        {
            ThrowIfDisposed();

            if (!OnMoveNext())
            {
                return false;
            }

            _recordNumber += 1;

            var recordTerminator = GetRecordTerminator();
            if (recordTerminator != null)
            {
                if (_recordTerminator == null)
                {
                    _recordTerminator = recordTerminator;
                }
                else if (_recordTerminator != recordTerminator)
                {
                    throw new CsvParsingException(
                        "일관성 없는 레코드 구분자입니다.",
                        null,
                        GetLineNumber(),
                        GetLinePosition());
                }
            }

            return true;
        }
        #endregion

        protected abstract string[] GetRecord();

        protected abstract int GetLineNumber();

        protected abstract int GetLinePosition();

        protected abstract CsvRecordTerminator? GetRecordTerminator();

        protected abstract bool OnMoveNext();

        protected virtual void Dispose(bool disposing)
        {
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(null);
            }
        }
    }
}
