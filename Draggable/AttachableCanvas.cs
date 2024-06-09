using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Macro_Plot.Draggable
{
    public class AttachableCanvas : Canvas
    {
        static AttachableCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AttachableCanvas), new FrameworkPropertyMetadata(typeof(AttachableCanvas)));
        }

        Hashtable pointsTable = [];

        public void RegisterPoints(PointCollection points, string tag)
        {
            if (tag == string.Empty) return;
            pointsTable.Add(tag, points);
        }

        public Point? GetNearestNeibor(Point target, string tag)
        {
            if (pointsTable.Count == 0) return null;
            if (!pointsTable.ContainsKey(tag)) return null;
            bool first = true;
            double distance = 0d;
            Stack<Point> pointStack = [];
            foreach (Point point in (PointCollection)pointsTable[tag]!)
            {
                if (first)
                {
                    distance = (point - target).Length;
                    first = false;
                    pointStack.Push(point);
                    continue;
                }
                if (distance > (point - target).Length)
                {
                    distance = (point - target).Length;
                    pointStack.Push(point);
                }
            }
            return pointStack.Pop();
        }
    }
}
