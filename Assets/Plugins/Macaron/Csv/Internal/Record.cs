using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Assembly-CSharp-Editor")]
#endif
namespace Macaron.Csv.Internal
{
    internal class Record<T> : ICsvRecord<T>
    {
        private readonly ICsvHeader<T> _header;
        private readonly int _recordNumber;
        private readonly string[] _fields;

        public Record(ICsvHeader<T> header, int recordNumber, IList<string> fields)
        {
            if (header == null)
            {
                throw new ArgumentNullException("header");
            }

            if (recordNumber < 1)
            {
                throw new ArgumentOutOfRangeException("recordNumber");
            }

            if (fields == null)
            {
                throw new ArgumentNullException("fields");
            }

            _header = header;
            _recordNumber = recordNumber;
            _fields = new string[fields.Count];
            fields.CopyTo(_fields, 0);
        }

        #region Implementation of ICsvRecord<T>
        public string this[int index]
        {
            get { return _fields[index]; }
            set { throw new NotSupportedException(); }
        }

        public int Count
        {
            get { return _fields.Length; }
        }

        public ICsvHeader<T> Header
        {
            get { return _header; }
        }

        public int RecordNumber
        {
            get { return _recordNumber; }
        }

        bool ICollection<string>.IsReadOnly
        {
            get { return true; }
        }

        public bool Contains(string item)
        {
            return (_fields as ICollection<string>).Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            _fields.CopyTo(array, arrayIndex);
        }

        public string Get(T columnName)
        {
            int index = _header.GetIndex(columnName);

            if (index == -1)
            {
                throw new ArgumentException(columnName + "은 유효한 열 이름이 아닙니다.", "columnName");
            }

            return _fields[index];
        }

        public IEnumerator<string> GetEnumerator()
        {
            return (_fields as IEnumerable<string>).GetEnumerator();
        }

        public int IndexOf(string item)
        {
            return Array.IndexOf(_fields, item);
        }

        void ICollection<string>.Add(string item)
        {
            throw new NotSupportedException();
        }

        void ICollection<string>.Clear()
        {
            throw new NotSupportedException();
        }

        void IList<string>.Insert(int index, string item)
        {
            throw new NotSupportedException();
        }

        bool ICollection<string>.Remove(string item)
        {
            throw new NotSupportedException();
        }

        void IList<string>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _fields.GetEnumerator();
        }
        #endregion
    }
}
