using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Macro_Plot.Node
{
    public sealed class Node : Control
    {
        static Node()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Node), new FrameworkPropertyMetadata(typeof(Node)));
        }

        public Node(NodeControl relativeNode)
        {
            ID = new Random().Next();
            RelativeNode = relativeNode;
        }

        public Node(int id, NodeControl relativeNode)
        {
            ID = id;
            RelativeNode = relativeNode;
            Width = 20;
            Height = 20;
        }

        public int ID { get; }

        public NodeControl RelativeNode { get; }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            NodeSelectedEventArgs args = new(NodeControl.NodeSelectedRoutedEvent, RelativeNode, this);
            RelativeNode.RaiseEvent(args);
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
            ((NodeCanvas)RelativeNode.Parent).OnNodeMouseDrag(this);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            ((NodeCanvas)RelativeNode.Parent).OnNodeMouseUp(this);
        }
    }
}
