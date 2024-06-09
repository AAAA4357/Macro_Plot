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
            MouseDown += Node_MouseDown;
            MouseUp += Node_MouseUp;
        }

        public Node(int id, NodeControl relativeNode)
        {
            ID = id;
            RelativeNode = relativeNode;
            Width = 20;
            Height = 20;
            MouseDown += Node_MouseDown;
            MouseUp += Node_MouseUp;
        }

        public static readonly DependencyProperty ConnectDirectionProperty = DependencyProperty.Register("ConnectDirection", typeof(double), typeof(Node), new PropertyMetadata(0d));

        public static readonly DependencyProperty NodeTagProperty = DependencyProperty.Register("NodeTag", typeof(string), typeof(Node), new PropertyMetadata(string.Empty));

        public double ConnectDirection
        {
            get { return (double)GetValue(ConnectDirectionProperty); }
            set { SetValue(ConnectDirectionProperty, value); }
        }

        public string NodeTag
        {
            get { return (string)GetValue(NodeTagProperty); }
            set { SetValue(NodeTagProperty, value); }
        }

        public int ID { get; }

        public NodeControl RelativeNode { get; }

        public List<NodeConnection> Connection { get; set; } = [];

        public Point Posistion { get; set; }

        public void RefreshLocation(Point position)
        {
            Canvas.SetLeft(this, position.X - ActualWidth / 2);
            Canvas.SetTop(this, position.Y - ActualHeight / 2);
            NodeCanvas canvas = (NodeCanvas)Parent;
            canvas.Children.Remove(this);
            canvas.Children.Add(this);
            foreach (NodeConnection connection in Connection) connection.RefreshNodeLine();
        }

        private void Node_MouseUp(object sender, MouseButtonEventArgs e) => ((NodeCanvas)Parent).OnNodeMouseUp(this);

        private void Node_MouseDown(object sender, MouseButtonEventArgs e) => ((NodeCanvas)Parent).OnNodeMouseDown(RelativeNode, this);
    }
}
