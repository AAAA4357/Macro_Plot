using Macro_Plot.Node;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;

namespace Macro_Plot.Utils
{
    /// <summary>
    /// 泛型类，定义最基础控件集合
    /// </summary>
    /// <typeparam name="T">控件泛型</typeparam>
    public class ControlCollection<T> : IDictionary<int, T>, IEnumerable where T : Control
    {
        readonly Hashtable inner_Conllection = [];

        public T this[int key]
        {
            get
            {
                if (!inner_Conllection.ContainsKey(key)) throw new ArgumentException("Key not defined in collection.");
                return (T)inner_Conllection[key]!;
            }
            set
            {
                if (!inner_Conllection.ContainsKey(key)) throw new ArgumentException("Key not defined in collection.");
                inner_Conllection[key] = value;
            }
        }

        public ICollection<int> Keys => (ICollection<int>)inner_Conllection.Keys;

        public ICollection<T> Values => (ICollection<T>)inner_Conllection.Values;

        public int Count => inner_Conllection.Count;

        public bool IsReadOnly => inner_Conllection.IsReadOnly;

        /// <summary>
        /// 添加的控件的父控件必须为 Canvas 或继承自 Canvas
        /// </summary>
        /// <param name="key">ID</param>
        /// <param name="value">控件</param>
        /// <exception cref="InvalidOperationException">当控件父控件不为 Canvas 或不继承自 Canvas 时抛出</exception>
        public void Add(int key, T value)
        {
            if (!value.Parent.GetType().IsAssignableTo(typeof(Canvas))) throw new InvalidOperationException("T 的父控件应继承 Canvas");
            inner_Conllection.Add(key, value);
        }

        [Obsolete("控件集合不支持键值对编辑", true)]
        public void Add(KeyValuePair<int, T> item) => throw new InvalidOperationException("控件集合不支持键值对编辑");

        public void Clear() => inner_Conllection.Clear();

        [Obsolete("控件集合不支持键值对编辑", true)]
        public bool Contains(KeyValuePair<int, T> item) => throw new InvalidOperationException("控件集合不支持键值对编辑");

        public bool ContainsKey(int key) => inner_Conllection.ContainsKey(key);

        [Obsolete("控件集合不支持键值对编辑", true)]
        public void CopyTo(KeyValuePair<int, T>[] array, int arrayIndex) => throw new InvalidOperationException("控件集合不支持键值对编辑");

        public void CopyTo(Array array, int index) => inner_Conllection.CopyTo(array, index);

        [Obsolete("控件集合不支持键值对编辑", true)]
        public IEnumerator<KeyValuePair<int, T>> GetEnumerator() => throw new InvalidOperationException("控件集合不支持键值对编辑");

        public bool Remove(int key)
        {
            if (!inner_Conllection.ContainsKey(key)) return false;
            inner_Conllection.Remove(key);
            return true;
        }

        [Obsolete("控件集合不支持键值对编辑", true)]
        public bool Remove(KeyValuePair<int, T> item) => throw new InvalidOperationException("控件集合不支持键值对编辑");

        public bool TryGetValue(int key, [MaybeNullWhen(false)] out T value)
        {
            if (!inner_Conllection.ContainsKey(key))
            {
                value = default;
                return false;
            }
            value = (T)inner_Conllection[key]!;
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() => inner_Conllection.GetEnumerator();
    }
}
