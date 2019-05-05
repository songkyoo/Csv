using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Assembly-CSharp-Editor")]
#endif
namespace Macaron.Csv.Internal.Headers
{
    internal partial class IndexHeader : ICsvHeader<int>
    {
        #region Fields
        private readonly string[] _columnNames;
        #endregion

        #region Constructors
        public IndexHeader(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            var columnNameGenerator = new ColumnNameGenerator();
            _columnNames = new string[count];

            for (int i = 0; i < count; ++i)
            {
                _columnNames[i] = columnNameGenerator.Next();
            }
        }
        #endregion

        #region Implementation of ICsvHeader<int>
        public string this[int index]
        {
            get { return _columnNames[index]; }
            set { throw new NotSupportedException(); }
        }

        public int Count
        {
            get { return _columnNames.Length; }
        }

        public bool Contains(string item)
        {
            return (_columnNames as ICollection<string>).Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            _columnNames.CopyTo(array, arrayIndex);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return (_columnNames as IEnumerable<string>).GetEnumerator();
        }

        public int GetIndex(int columnName)
        {
            if (columnName < 0 || columnName >= _columnNames.Length)
            {
                return -1;
            }

            return columnName;
        }

        public int IndexOf(string item)
        {
            return Array.IndexOf(_columnNames, item);
        }

        bool ICollection<string>.IsReadOnly
        {
            get { return true; }
        }

        void ICollection<string>.Add(string item)
        {
            throw new NotSupportedException();
        }

        void ICollection<string>.Clear()
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _columnNames.GetEnumerator();
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
        #endregion
    }
}
