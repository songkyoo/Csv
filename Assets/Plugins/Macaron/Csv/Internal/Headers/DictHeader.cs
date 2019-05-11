using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Assembly-CSharp-Editor")]
#endif
namespace Macaron.Csv.Internal.Headers
{
    internal class DictHeader : ICsvHeader<string>
    {
        #region Fields
        private string[] _columnNames;
        private Dictionary<string, int> _columnNameToIndex;
        #endregion

        #region Constructors
        public DictHeader(IList<string> columnNames, IEqualityComparer<string> comparer)
        {
            if (columnNames == null)
            {
                throw new ArgumentNullException("columnNames");
            }

            var count = columnNames.Count;

            for (int i = 0; i < count; ++i)
            {
                if (columnNames[i] == null)
                {
                    throw new ArgumentException("null 요소를 가질 수 없습니다.", "columnNames");
                }
            }

            var columnNametoIndex = new Dictionary<string, int>(count, comparer);

            try
            {
                for (int i = 0; i < count; ++i)
                {
                    if (columnNames[i].Length != 0)
                    {
                        columnNametoIndex.Add(columnNames[i], i);
                    }
                }
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("중복된 이름을 가질 수 없습니다.", "columnNames");
            }

            _columnNameToIndex = columnNametoIndex;
            _columnNames = new string[columnNames.Count];
            columnNames.CopyTo(_columnNames, 0);
        }
        #endregion

        #region Implementation of ICsvHeader<string>
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

        public int GetIndex(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                return -1;
            }

            int index;
            return _columnNameToIndex.TryGetValue(columnName, out index) ? index : -1;
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
