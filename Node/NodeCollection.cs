using Macro_Plot.Utils;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Macro_Plot.Node
{
    public class NodeCollection(NodeControl relativeControl) : ControlCollection<Node>, ICollection
    {
        public NodeControl RelativeControl { get; } = relativeControl;

        public bool IsSynchronized => false;

        public object SyncRoot => this;

        public int AddNode(Point position)
        {
            if (position.X > RelativeControl.ActualWidth || position.Y > RelativeControl.ActualHeight) throw new ArgumentException("节点位于控件之外");
            int id = new Random().Next();
            Node node = new(id, RelativeControl)
            {
                Style = RelativeControl.NodeStyle
            };
            ((Canvas)RelativeControl.Parent).Children.Add(node);
            Canvas.SetLeft(node, Canvas.GetLeft(RelativeControl) + position.X - node.Width / 2);
            Canvas.SetTop(node, Canvas.GetTop(RelativeControl) + position.Y - node.Height / 2);
            Add(id, node);
            return id;
        }

        public bool RemoveNode(int id) => Remove(id);

        public Node FindNode(int id) => this[id];
    }
}
