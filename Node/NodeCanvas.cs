using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Macro_Plot.Node
{
    public class NodeCanvas : Canvas
    {
        static NodeCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeCanvas), new FrameworkPropertyMetadata(typeof(NodeCanvas)));
        }

        public NodeControlCollection NodeControls { get; set; } = [];

        public List<NodeConnection> NodeConnections { get; set; } = [];

        NodeControl? connectControl;

        bool isConnecting;

        Node? sourceNode;

        Point? connectStartPoint;

        NodeConnection? connection;

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            isConnecting = false;
            connectControl = null;
            sourceNode = null;
            connectStartPoint = null;
            connection?.RemoveLine();
            connection = null;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!isConnecting) return;
            var source = sourceNode!.Posistion + new Vector(GetLeft(connectControl), GetTop(connectControl));
            var target = source - Mouse.GetPosition(this);
            var direction = Math.Atan2(target.Y, target.X);
            if (connectStartPoint.HasValue) connection!.RefreshLine(source, sourceNode.ConnectDirection, Mouse.GetPosition(this) + new Vector(10d * Math.Cos(direction), 10d * Math.Sin(direction)), direction);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            isConnecting = false;
            connectControl = null;
            sourceNode = null;
            connectStartPoint = null;
            connection?.RemoveLine();
            connection = null;
        }

        public void RegisterNodeControl(int ID, NodeControl control) => NodeControls.Add(ID, control);

        public void OnNodeMouseDown(NodeControl targetControl, Node sourceNode)
        {
            isConnecting = true;
            this.sourceNode = sourceNode;
            connectControl = targetControl;
            NodeStartConnectEventArgs args = new(NodeControl.NodeStartConnectRoutedEvent, connectControl, sourceNode);
            connectControl.RaiseEvent(args);
            Point start = sourceNode!.Posistion + new Vector(GetLeft(connectControl), GetTop(connectControl));
            connectStartPoint = start;
            connection = new(this);
            var target = (Mouse.GetPosition(this) - start);
            var direction = Math.Atan2(target.Y, target.X);
            connection!.RefreshLine(start, sourceNode.ConnectDirection, Mouse.GetPosition(this) + new Vector(10d * Math.Cos(direction), 10d * Math.Sin(direction)), direction);
        }

        public void OnNodeMouseUp(Node targetNode)
        {
            if (sourceNode is null) return;
            isConnecting = false;
            if (targetNode != sourceNode && sourceNode.NodeTag == targetNode.NodeTag && targetNode.RelativeNode != sourceNode.RelativeNode)
            {
                if (NodeConnections.Find(x => (x.Source == sourceNode && x.Target == targetNode) || (x.Target == sourceNode && x.Source == targetNode)) is not null)
                {
                    connection!.RemoveLine();
                    return;
                }
                NodeCompleteConnectEventArgs args = new(NodeControl.NodeCompleteConnectRoutedEvent, connectControl!, sourceNode, targetNode);
                connectControl!.RaiseEvent(args);
                args = new(NodeControl.NodeCompleteConnectRoutedEvent, targetNode.RelativeNode, sourceNode, targetNode);
                targetNode.RelativeNode.RaiseEvent(args);
                AddConnection(sourceNode, targetNode, connection!);
                sourceNode = null;
                connectControl = null;
                connectStartPoint = null;
                connection = null;
                return;
            }
            sourceNode = null;
            connectControl = null;
            connectStartPoint = null;
            connection?.RemoveLine();
            connection = null;
        }

        public void AddConnection(Node source, Node target, NodeConnection connection)
        {
            source.Connection.Add(connection);
            target.Connection.Add(connection);
            connection.Source = source;
            connection.Target = target;
            NodeConnections.Add(connection);
            connection.RefreshNodeLine();
        }

        public void RemoveConnection(NodeConnection connection)
        {
            connection.Source!.Connection.Remove(connection);
            connection.Target!.Connection.Remove(connection);
            NodeConnections.Remove(connection);
            connection.RemoveLine();
        }
    }
}
