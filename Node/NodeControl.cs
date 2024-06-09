using Macro_Plot.Draggable;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Macro_Plot.Node
{
    public class NodeControl : DraggableControl
    {
        static NodeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeControl), new FrameworkPropertyMetadata(typeof(NodeControl)));
        }

        public NodeControl()
        {
            ID = new Random().Next();
            Nodes = new(this);
            Loaded += NodeControl_Loaded;
            OnDragControl += NodeControl_OnDragControl;
        }

        public static readonly DependencyProperty NodeStyleProperty = DependencyProperty.Register("NodeStyle", typeof(Style), typeof(NodeControl), new PropertyMetadata(new Style()));

        public static readonly DependencyProperty NodeConnectionStyleProperty = DependencyProperty.Register("NodeConnectionStyle", typeof(Style), typeof(NodeControl), new PropertyMetadata(new Style()));

        public static readonly DependencyProperty NodePointsProperty = DependencyProperty.Register("NodePoints", typeof(NodePointCollection), typeof(NodeControl), new PropertyMetadata(new NodePointCollection()));

        public static readonly RoutedEvent NodeStartConnectRoutedEvent = EventManager.RegisterRoutedEvent("NodeStartConnect", RoutingStrategy.Bubble, typeof(NodeStartConnectEventHandler), typeof(NodeCanvas));

        public static readonly RoutedEvent NodeCompleteConnectRoutedEvent = EventManager.RegisterRoutedEvent("NodeCompleteConnect", RoutingStrategy.Bubble, typeof(NodeCompleteConnectEventHandler), typeof(NodeCanvas));

        public Style NodeStyle
        {
            get { return (Style)GetValue(NodeStyleProperty); }
            set { SetValue(NodeStyleProperty, value); }
        }

        public Style NodeConnectionStyle
        {
            get { return (Style)GetValue(NodeConnectionStyleProperty); }
            set { SetValue(NodeConnectionStyleProperty, value); }
        }

        public NodePointCollection NodePoints
        {
            get { return (NodePointCollection)GetValue(NodePointsProperty); }
            set { SetValue(NodePointsProperty, value); }
        }

        public event NodeStartConnectEventHandler NodeStartConnect
        {
            add { AddHandler(NodeStartConnectRoutedEvent, value); }
            remove { RemoveHandler(NodeStartConnectRoutedEvent, value); }
        }

        public event NodeCompleteConnectEventHandler NodeCompleteConnect
        {
            add { AddHandler(NodeCompleteConnectRoutedEvent, value); }
            remove { RemoveHandler(NodeCompleteConnectRoutedEvent, value); }
        }

        public int ID { get; }

        public NodeCollection Nodes { get; set; }

        private NodeCanvas? RelativeCanvas { get; set; }

        Point Position
        {
            get => new(double.IsNaN(Canvas.GetLeft(this)) ? 0d : Canvas.GetLeft(this), double.IsNaN(Canvas.GetTop(this)) ? 0d : Canvas.GetTop(this));
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            foreach (Node node in Nodes.Values) node.RefreshLocation(Position + (Vector)node.Posistion);
        }

        private void NodeControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Parent.GetType() != typeof(NodeCanvas)) throw new InvalidOperationException("节点控件必须放置于节点画布中");
            RelativeCanvas = (NodeCanvas)Parent;
            RelativeCanvas.RegisterNodeControl(ID, this);
            PointCollection points = [];
            for (int i = 0; i < NodePoints.Count; i++)
            {
                NodePoint position = NodePoints[i];
                if (points.Contains(position.NodePosition)) throw new ArgumentException("多个相同位置位置节点");
                points.Add(position.NodePosition);
                Nodes.AddNode(position);
            }
        }

        private void NodeControl_OnDragControl(object? source, DragControlEventArgs e)
        {
            foreach (Node node in Nodes.Values) node.RefreshLocation(Position + (Vector)node.Posistion);
        }
    }
}