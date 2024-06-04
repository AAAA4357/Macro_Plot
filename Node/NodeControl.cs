using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Macro_Plot.Node
{
    public class NodeControl : ContentControl
    {
        static NodeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeControl), new FrameworkPropertyMetadata(typeof(NodeControl)));
        }

        public NodeControl()
        {
            Nodes = new(this);
            Loaded += NodeControl_Loaded;
        }

        public static readonly DependencyProperty NodeStyleProperty = DependencyProperty.Register("NodeStyle", typeof(Style), typeof(NodeControl), new PropertyMetadata(new Style()));

        public static readonly DependencyProperty NodeConnectionStyleProperty = DependencyProperty.Register("NodeConnectionStyle", typeof(Style), typeof(NodeControl), new PropertyMetadata(new Style()));

        public static readonly DependencyProperty NodePointsProperty = DependencyProperty.Register("NodePoints", typeof(PointCollection), typeof(NodeControl), new PropertyMetadata(new PointCollection()));

        public static readonly RoutedEvent NodeSelectedRoutedEvent = EventManager.RegisterRoutedEvent("NodeSelected", RoutingStrategy.Bubble, typeof(NodeSelectedEventHandler), typeof(Node));

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

        public PointCollection NodePoints
        {
            get { return (PointCollection)GetValue(NodePointsProperty); }
            set { SetValue(NodePointsProperty, value); }
        }

        public event NodeSelectedEventHandler NodeSelected
        {
            add { AddHandler(NodeSelectedRoutedEvent, value); }
            remove { RemoveHandler(NodeSelectedRoutedEvent, value); }
        }

        private NodeCollection Nodes { get; set; }

        private void NodeControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Parent.GetType() != typeof(NodeCanvas)) throw new InvalidOperationException("节点控件必须放置于节点画布中");
            PointCollection points = [];
            for (int i = 0; i < NodePoints.Count; i++)
            {
                Point position = NodePoints[i];
                if (points.Contains(position)) throw new ArgumentException("多个相同位置位置节点");
                points.Add(position);
                Nodes.AddNode(position);
            }
        }

        private const double SnapDistance = 20d;
        private Line? _line;
        private Point _startPoint;
        private UIElement? _source;
        private UIElement? _target;
        private bool _isDrawing;

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FinishConnect(e.GetPosition(this));
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateLine(e.GetPosition(this));
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            FinishConnect(e.GetPosition(this));
        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(this);
            _isDrawing = true;
            _source = (UIElement)sender;
        }

        private void UpdateLine(Point point)
        {
            if (_isDrawing)
            {
                if (_line == null)
                {
                    _line = new Line()
                    {
                        Stroke = Brushes.Red,
                        StrokeThickness = 4,

                        X1 = _startPoint.X,
                        Y1 = _startPoint.Y,
                        X2 = point.X,
                        Y2 = point.Y,
                    };
                    ((Canvas)Parent).Children.Add(_line);
                }
                else
                {
                    _line.X2 = point.X;
                    _line.Y2 = point.Y;
                }
                List<Ellipse> nodes = ((Canvas)Parent).Children.OfType<Ellipse>().Where(x => x != _source).ToList();
                for (int i = 0; i < nodes.Count; i++)
                {
                    double x = (double)nodes[i].GetValue(Canvas.LeftProperty);
                    Point center = new Point((double)nodes[i].GetValue(Canvas.LeftProperty) + nodes[i].Width / 2, (double)nodes[i].GetValue(Canvas.TopProperty) + nodes[i].Height / 2);

                    if ((center - point).Length <= SnapDistance)
                    {
                        _target = nodes[i];

                        _line.X2 = center.X;
                        _line.Y2 = center.Y;

                        Console.WriteLine("找到吸附点");
                        break;
                    }
                    else
                    {
                        _target = null;
                    }
                }
            }
        }

        private void FinishConnect(Point endPoint)
        {
            if (_line != null)
            { 
                if (_target != null && _target is Ellipse node)
                {
                    endPoint = new Point((double)node.GetValue(Canvas.LeftProperty) + node.Width / 2,
                                        (double)node.GetValue(Canvas.TopProperty) + node.Height / 2);
                }

                _line.X2 = endPoint.X;
                _line.Y2 = endPoint.Y;

                if (_target != null)
                {

                }
                else
                {
                    ((Canvas)Parent).Children.Remove(_line);
                }

                _isDrawing = false;
                _line = null;
                _target = null;
            }
        }
    }
}