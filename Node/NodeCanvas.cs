using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static readonly DependencyProperty NodeControlsProperty = DependencyProperty.Register("NodeControls", typeof(NodeControlCollection), typeof(NodeCanvas), new PropertyMetadata(new NodeControlCollection()));

        public static readonly RoutedEvent NodeStartConnectRoutedEvent = EventManager.RegisterRoutedEvent("NodeStartConnect", RoutingStrategy.Bubble, typeof(NodeStartConnectEventHandler), typeof(NodeCanvas));

        public static readonly RoutedEvent NodeConnectRoutedEvent = EventManager.RegisterRoutedEvent("NodeConnect", RoutingStrategy.Bubble, typeof(NodeConnectEventHandler), typeof(NodeCanvas));

        public static readonly RoutedEvent NodeCompleteConnectRoutedEvent = EventManager.RegisterRoutedEvent("NodeCompleteConnect", RoutingStrategy.Bubble, typeof(NodeCompleteConnectEventHandler), typeof(NodeCanvas));

        public NodeControlCollection NodeControls
        {
            get { return (NodeControlCollection)GetValue(NodeControlsProperty); }
            set { SetValue(NodeControlsProperty, value); }
        }

        public event NodeStartConnectEventHandler NodeStartConnect
        {
            add { AddHandler(NodeStartConnectRoutedEvent, value); }
            remove { RemoveHandler(NodeStartConnectRoutedEvent, value); }
        }

        public event NodeConnectEventHandler NodeConnect
        {
            add { AddHandler(NodeConnectRoutedEvent, value); }
            remove { RemoveHandler(NodeConnectRoutedEvent, value); }
        }

        public event NodeCompleteConnectEventHandler NodeCompleteConnect
        {
            add { AddHandler(NodeCompleteConnectRoutedEvent, value); }
            remove { RemoveHandler(NodeCompleteConnectRoutedEvent, value); }
        }

        bool isConnecting;

        Node? sourceNode;

        public void OnNodeMouseDrag(Node sourceNode)
        {
            isConnecting = true;
            this.sourceNode = sourceNode;
            NodeStartConnectEventArgs args = new(NodeStartConnectRoutedEvent, this, sourceNode);
            RaiseEvent(args);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isConnecting)
            {
                NodeConnectEventArgs args = new(NodeConnectRoutedEvent, this, sourceNode!, Mouse.GetPosition(this));
                RaiseEvent(args);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            isConnecting = false;
            sourceNode = null;
        }

        public void OnNodeMouseUp(Node targetNode)
        {
            isConnecting = false;
            if (targetNode != sourceNode!)
            {
                NodeCompleteConnectEventArgs args = new(NodeCompleteConnectRoutedEvent, this, sourceNode!, targetNode);
                RaiseEvent(args);
            }
            sourceNode = null;
        }
    }
}
