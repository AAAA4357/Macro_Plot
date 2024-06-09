using System.Collections;

namespace Macro_Plot.Node
{
    public class NodePointCollection : IList<NodePoint>, IEnumerable<NodePoint>, IList, IEnumerable
    {
        List<NodePoint> innerList = [];

        public NodePoint this[int index] { get => innerList[index]; set => innerList[index] = value; }

        object? IList.this[int index] { get => innerList[index]; set => innerList[index] = (NodePoint)value!; }

        public int Count => innerList.Count;

        public bool IsReadOnly => false;

        public bool IsFixedSize => false;

        public bool IsSynchronized => false;

        public object SyncRoot => this;

        public void Add(NodePoint item) => innerList.Add(item);

        public int Add(object? value)
        {
            innerList.Add((NodePoint)value!);
            return innerList.IndexOf((NodePoint)value!);
        }

        public void Clear() => innerList.Clear();

        public bool Contains(NodePoint item) => innerList.Contains(item);

        public bool Contains(object? value) => innerList.Contains((NodePoint)value!);

        public void CopyTo(NodePoint[] array, int arrayIndex) => innerList.CopyTo(array, arrayIndex);

        public void CopyTo(Array array, int index) => innerList.CopyTo((NodePoint[])array, index);

        public IEnumerator<NodePoint> GetEnumerator() => innerList.GetEnumerator();

        public int IndexOf(NodePoint item) => innerList.IndexOf(item);

        public int IndexOf(object? value) => innerList.IndexOf((NodePoint)value!);

        public void Insert(int index, NodePoint item) => innerList.Insert(index, item);

        public void Insert(int index, object? value) => innerList.Insert(index, (NodePoint)value!);

        public bool Remove(NodePoint item) => innerList.Remove(item);

        public void Remove(object? value) => innerList.Remove((NodePoint)value!);

        public void RemoveAt(int index) => innerList.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => innerList.GetEnumerator();
    }
}
