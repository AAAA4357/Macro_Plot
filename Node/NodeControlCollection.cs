using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Macro_Plot.Node
{
    public class NodeControlCollection() : IDictionary<int, NodeControl>, ICollection, IEnumerable
    {
        private readonly Hashtable inner_Conllection = [];

        public NodeControl this[int key]
        {
            get
            {
                if (!inner_Conllection.ContainsKey(key)) throw new ArgumentException("Key not defined in collection.");
                return (NodeControl)inner_Conllection[key]!;
            }
            set
            {
                if (!inner_Conllection.ContainsKey(key)) throw new ArgumentException("Key not defined in collection.");
                inner_Conllection[key] = value;
            }
        }

        public ICollection<int> Keys => (ICollection<int>)inner_Conllection.Keys;

        public ICollection<NodeControl> Values => (ICollection<NodeControl>)inner_Conllection.Values;

        public int Count => inner_Conllection.Count;

        public bool IsReadOnly => inner_Conllection.IsReadOnly;
        
        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;

        public void Add(int key, NodeControl value) => inner_Conllection.Add(key, value);

        public void Add(KeyValuePair<int, NodeControl> item) => throw new InvalidOperationException("Node Collection 不支持键值对编辑");

        public void Clear() => inner_Conllection.Clear();

        public bool Contains(KeyValuePair<int, NodeControl> item) => throw new InvalidOperationException("Node Collection 不支持键值对编辑");

        public bool ContainsKey(int key) => inner_Conllection.ContainsKey(key);

        public void CopyTo(KeyValuePair<int, NodeControl>[] array, int arrayIndex) => throw new InvalidOperationException("Node Collection 不支持键值对编辑");

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<int, NodeControl>> GetEnumerator() => throw new InvalidOperationException("Node Collection 不支持键值对编辑");

        public bool Remove(int key)
        {
            if (!inner_Conllection.ContainsKey(key)) return false;
            inner_Conllection.Remove(key); 
            return true;
        }

        public bool Remove(KeyValuePair<int, NodeControl> item) => throw new InvalidOperationException("Node Collection 不支持键值对编辑");

        public bool TryGetValue(int key, [MaybeNullWhen(false)] out NodeControl value)
        {
            if (!inner_Conllection.ContainsKey(key))
            {
                value = default;
                return false;
            }
            value = (NodeControl)inner_Conllection[key]!;
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() => inner_Conllection.GetEnumerator();
    }
}
