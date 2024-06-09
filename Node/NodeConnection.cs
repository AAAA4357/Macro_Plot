using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Macro_Plot.Node
{
    public class NodeConnection(NodeCanvas canvas)
    {
        public Node? Source { get; set; }

        public Node? Target { get; set; }

        NodeCanvas canvas = canvas;

        Path ConnectionLine(Point point1, Point point2, Point point3, Point point4) => new()
        {
            Stroke = new SolidColorBrush(Colors.Black),
            StrokeThickness = 4,
            Data = new PathGeometry()
            {
                Figures =
                {
                    new()
                    {
                        StartPoint = point1,
                        Segments =
                        {
                            new BezierSegment()
                            {
                                Point1 = point2,
                                Point2 = point3,
                                Point3 = point4
                            }
                        }
                    }
                }
            }
        };

        Path? linePath;

        public void RefreshLine(Point source, double sourceDirection, Point destination, double destinationDirection)
        {
            RemoveLine();
            var distance = (destination - source).Length;
            Point point1, point2, point3, point4;
            point1 = source;
            point4 = destination;
            point2 = source + new Vector(distance * 0.5 * Math.Cos(sourceDirection), distance * 0.5 * Math.Sin(sourceDirection));
            point3 = destination + new Vector(distance * 0.5 * Math.Cos(destinationDirection), distance * 0.5 * Math.Sin(destinationDirection));
            linePath = ConnectionLine(point1, point2, point3, point4);
            linePath.MouseRightButtonDown += (s, e) =>
            {
                canvas.RemoveConnection(this);
            };
            canvas.Children.Add(linePath);
        }

        public void RefreshNodeLine()
        {
            RefreshLine(new Point(Canvas.GetLeft(Source!.RelativeNode), Canvas.GetTop(Source!.RelativeNode)) + (Vector)Source!.Posistion, Source!.ConnectDirection, new Point(Canvas.GetLeft(Target!.RelativeNode), Canvas.GetTop(Target!.RelativeNode)) + (Vector)Target!.Posistion, Target!.ConnectDirection);
        }

        public void RemoveLine()
        {
            if (linePath is not null)
            {
                canvas.Children.Remove(linePath);
                linePath = null;
            }
        }
    }
}
