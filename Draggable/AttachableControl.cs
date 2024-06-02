using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Macro_Plot.Draggable
{
    public class AttachableControl : DraggableControl
    {
        static AttachableControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AttachableControl), new FrameworkPropertyMetadata(typeof(AttachableControl)));
        }

        public static readonly DependencyProperty IsAttachableProperty = DependencyProperty.Register("IsAttachable", typeof(bool), typeof(AttachableControl), new PropertyMetadata(true));

        public static readonly DependencyProperty AttachPointProperty = DependencyProperty.Register("AttachPoint", typeof(Point), typeof(AttachableControl), new PropertyMetadata(new Point(0, 0)));

        public static readonly DependencyProperty AttachRangeProperty = DependencyProperty.Register("AttachRange", typeof(double), typeof(AttachableControl), new PropertyMetadata(20d));

        public static readonly DependencyProperty AttachMinThresholdProperty = DependencyProperty.Register("AttachMinThreshold", typeof(double), typeof(AttachableControl), new PropertyMetadata(20d));

        public static readonly DependencyProperty AttachDurationProperty = DependencyProperty.Register("AttachDuration", typeof(Duration), typeof(AttachableControl), new PropertyMetadata(new Duration(new(0, 0, 1))));

        public static readonly DependencyProperty AttachEasingFunctionProperty = DependencyProperty.Register("AttachEasingFunction", typeof(EasingFunctionBase), typeof(AttachableControl), new PropertyMetadata(new SineEase()));

        public static readonly RoutedEvent AttachStartRoutedEvent = EventManager.RegisterRoutedEvent("OnAttachStart", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AttachableControl));

        public static readonly RoutedEvent AttachRoutedEvent = EventManager.RegisterRoutedEvent("OnAttach", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AttachableControl));

        public static readonly RoutedEvent AttachCompleteRoutedEvent = EventManager.RegisterRoutedEvent("OnAttachComplete", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AttachableControl));

        public static readonly RoutedEvent UnAttachRoutedEvent = EventManager.RegisterRoutedEvent("OnUnAttach", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AttachableControl));

        public bool IsAttachable
        {
            get { return (bool)GetValue(IsAttachableProperty); }
            set { SetValue(IsAttachableProperty, value); }
        }

        public Point AttachPoint
        {
            get { return (Point)GetValue(AttachPointProperty); }
            set { SetValue(AttachPointProperty, value); }
        }

        public double AttachRange
        {
            get { return (double)GetValue(AttachRangeProperty); }
            set { SetValue(AttachRangeProperty, value); }
        }

        public double AttachMinThreshold
        {
            get { return (double)GetValue(AttachMinThresholdProperty); }
            set { SetValue(AttachMinThresholdProperty, value); }
        }

        public Duration AttachDuration
        {
            get { return (Duration)GetValue(AttachDurationProperty); }
            set { SetValue(AttachDurationProperty, value); }
        }

        public EasingFunctionBase AttachEasingFunction
        {
            get { return (EasingFunctionBase)GetValue(AttachEasingFunctionProperty); }
            set { SetValue(AttachEasingFunctionProperty, value); }
        }

        public event RoutedEventHandler OnAttachStart
        {
            add { AddHandler(AttachStartRoutedEvent, value); }
            remove { RemoveHandler(AttachStartRoutedEvent, value); }
        }

        public event RoutedEventHandler OnAttach
        {
            add { AddHandler(AttachRoutedEvent, value); }
            remove { RemoveHandler(AttachRoutedEvent, value); }
        }

        public event RoutedEventHandler OnAttachComplete
        {
            add { AddHandler(AttachCompleteRoutedEvent, value); }
            remove { RemoveHandler(AttachCompleteRoutedEvent, value); }
        }

        bool IsAttaching { get; set; }

        Vector? AttachStart_CursorPosition { get; set; }

        Storyboard? AttachStoryboard { get; set; }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (AttachStart_CursorPosition.HasValue && (Vector)Mouse.GetPosition((Canvas)Parent) != AttachStart_CursorPosition.Value)
            {
                UnAttach();
                RoutedEventArgs args = new(UnAttachRoutedEvent, this);
                RaiseEvent(args);
                base.OnMouseMove(e);
                return;
            }
            if (!IsAttaching)
            {
                base.OnMouseMove(e);
            }
            else
            {
                RoutedEventArgs args = new(AttachRoutedEvent, this);
                RaiseEvent(args);
            }
            if (AttachStart_CursorPosition is not null) return;
            double distance = (new Point(Canvas.GetLeft(this), Canvas.GetTop(this)) - AttachPoint).Length;
            if (distance <= AttachRange)
            {
                Attach();
                RoutedEventArgs args = new(AttachStartRoutedEvent, this);
                RaiseEvent(args);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (IsAttaching)
            {
                UnAttach();
                RoutedEventArgs args = new(UnAttachRoutedEvent, this);
                RaiseEvent(args);
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (IsAttaching)
            {
                UnAttach();
                RoutedEventArgs args = new(UnAttachRoutedEvent, this);
                RaiseEvent(args);
            }
        }

        public virtual void Attach()
        {
            IsAttaching = true;
            AttachStoryboard = new();
            DoubleAnimation AttachXAnimation = new()
            {
                From = Canvas.GetLeft(this),
                To = AttachPoint.X,
                Duration = AttachDuration,
                EasingFunction = AttachEasingFunction
            };
            Storyboard.SetTargetProperty(AttachXAnimation, new(Canvas.LeftProperty));
            Storyboard.SetTarget(AttachXAnimation, this);
            AttachStoryboard.Children.Add(AttachXAnimation);
            DoubleAnimation AttachYAnimation = new()
            {
                From = Canvas.GetTop(this),
                To = AttachPoint.Y,
                Duration = AttachDuration,
                EasingFunction = AttachEasingFunction
            };
            Storyboard.SetTargetProperty(AttachYAnimation, new(Canvas.TopProperty));
            Storyboard.SetTarget(AttachYAnimation, this);
            AttachStoryboard.Children.Add(AttachYAnimation);
            AttachStoryboard.Begin();
            AttachStart_CursorPosition = (Vector)Mouse.GetPosition((Canvas)Parent);
            AttachStoryboard.Completed += (object? sender, EventArgs e) =>
            {
                IsAttaching = false;
                AttachStoryboard.Stop();
                AttachStoryboard = null;
                AttachStart_CursorPosition = null;
                RoutedEventArgs args = new(AttachCompleteRoutedEvent, this);
                RaiseEvent(args);
            };
        }

        public virtual void UnAttach()
        {
            IsAttaching = false;
            AttachStoryboard?.Stop();
            AttachStoryboard = null;
            AttachStart_CursorPosition = null;
        }
    }
}
