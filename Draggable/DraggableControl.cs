using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Macro_Plot.Draggable
{
    public class DraggableControl : ContentControl
    {
        static DraggableControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DraggableControl), new FrameworkPropertyMetadata(typeof(DraggableControl)));
        }

        public static readonly DependencyProperty IsDraggableProperty = DependencyProperty.Register("IsDraggable", typeof(bool), typeof(DraggableControl), new PropertyMetadata(true));

        public static readonly RoutedEvent DragControlEvent = EventManager.RegisterRoutedEvent("OnDragControl", RoutingStrategy.Bubble, typeof(DragControlHandler), typeof(DraggableControl));

        public bool IsDraggable
        {
            get { return (bool)GetValue(IsDraggableProperty); }
            set { SetValue(IsDraggableProperty, value); }
        }

        public event DragControlHandler OnDragControl
        {
            add { AddHandler(DragControlEvent, value); }
            remove { RemoveHandler(DragControlEvent, value); }
        }

        protected bool IsDragging { get; set; }

        protected Vector? MousePosition { get; set; }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (Parent.GetType().IsAssignableFrom(typeof(Canvas)) || !IsDraggable) return;
            IsDragging = true;
            MousePosition = (Vector)Mouse.GetPosition(this);
            UIElementCollection collection = ((Canvas)Parent).Children;
            collection.Remove(this);
            collection.Add(this);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (IsDragging)
            {
                Point mouse_parent = Mouse.GetPosition((FrameworkElement)Parent);
                Vector destination = (Vector)mouse_parent;
                destination -= MousePosition!.Value;
                Canvas.SetLeft(this, destination.X);
                Canvas.SetTop(this, destination.Y);
                DragControlEventArgs args = new(DragControlEvent, this, Mouse.GetPosition(this), TranslatePoint(new(), (Canvas)Parent));
                RaiseEvent(args);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            IsDragging = false;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            IsDragging = false;
        }
    }
}
