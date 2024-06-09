using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Timers;
using Timer = System.Timers.Timer;
using Macro_Plot.Utils;

namespace Macro_Plot.Draggable
{
    public class AttachableControl : DraggableControl
    {
        static AttachableControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AttachableControl), new FrameworkPropertyMetadata(typeof(AttachableControl)));
        }

        public AttachableControl()
        {
            AttachTimer = new(AttachMinMouseOverTime * 1000)
            {
                Enabled = false,
                AutoReset = false
            };
            AttachTimer.Elapsed += (object? sender, ElapsedEventArgs e) =>
            {
                RoutedEventArgs args = new(AttachStartRoutedEvent, this);
                RaiseEvent(args);
                IsAttaching = true;
                Dispatcher.Invoke(() =>
                {
                    AttachStoryboard?.Begin();
                });
                ((Timer)sender!).Enabled = false;
            };
            Loaded += AttachableControl_Loaded;
        }

        public static readonly DependencyProperty IsAttachableProperty = DependencyProperty.Register("IsAttachable", typeof(bool), typeof(AttachableControl), new PropertyMetadata(true));

        public static readonly DependencyProperty AttachRangeProperty = DependencyProperty.Register("AttachRange", typeof(double), typeof(AttachableControl), new PropertyMetadata(20d));

        public static readonly DependencyProperty AttachMinThresholdProperty = DependencyProperty.Register("AttachMinThreshold", typeof(double), typeof(AttachableControl), new PropertyMetadata(1d));

        public static readonly DependencyProperty AttachMinMouseOverTimeProperty = DependencyProperty.Register("AttachMinMouseOverTime", typeof(double), typeof(AttachableControl), new PropertyMetadata(0.2d));

        public static readonly DependencyProperty AttachDurationProperty = DependencyProperty.Register("AttachDuration", typeof(Duration), typeof(AttachableControl), new PropertyMetadata(new Duration(new(0, 0, 0, 0, 500))));

        public static readonly DependencyProperty AttachEasingFunctionProperty = DependencyProperty.Register("AttachEasingFunction", typeof(EasingFunctionBase), typeof(AttachableControl), new PropertyMetadata(new SineEase()));

        public static readonly DependencyProperty AttachTagProperty = DependencyProperty.Register("AttachTag", typeof(string), typeof(AttachableControl), new PropertyMetadata(string.Empty));

        public static readonly RoutedEvent AttachStartRoutedEvent = EventManager.RegisterRoutedEvent("OnAttachStart", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AttachableControl));

        public static readonly RoutedEvent AttachRoutedEvent = EventManager.RegisterRoutedEvent("OnAttach", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AttachableControl));

        public static readonly RoutedEvent AttachCompleteRoutedEvent = EventManager.RegisterRoutedEvent("OnAttachComplete", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AttachableControl));

        public static readonly RoutedEvent UnAttachRoutedEvent = EventManager.RegisterRoutedEvent("OnUnAttach", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AttachableControl));

        public bool IsAttachable
        {
            get { return (bool)GetValue(IsAttachableProperty); }
            set { SetValue(IsAttachableProperty, value); }
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

        public double AttachMinMouseOverTime
        {
            get { return (double)GetValue(AttachMinMouseOverTimeProperty); }
            set { SetValue(AttachMinMouseOverTimeProperty, value); }
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

        public event RoutedEventHandler OnUnAttach
        {
            add { AddHandler(UnAttachRoutedEvent, value); }
            remove { RemoveHandler(UnAttachRoutedEvent, value); }
        }

        public string AttachTag
        {
            get { return (string)GetValue(AttachTagProperty); }
            set { SetValue(AttachTagProperty, value); }
        }

        AttachableCanvas? RelativeCanvas;

        Point? AttachPoint;

        bool IsAttaching;

        Vector? AttachStart_CursorPosition;

        Storyboard? AttachStoryboard;

        Timer AttachTimer;

        Point Position
        {
            get => new(double.IsNaN(Canvas.GetLeft(this)) ? 0d : Canvas.GetLeft(this), double.IsNaN(Canvas.GetTop(this)) ? 0d : Canvas.GetTop(this));
        }

        protected bool IsAttached { get; private set; }

        protected void StartTimer()
        {
            AttachTimer.Enabled = true;
            AttachTimer.Start();
        }

        protected void StopTimer()
        {
            AttachTimer.Stop();
            AttachTimer.Enabled = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!IsDragging) return;
            AttachPoint = RelativeCanvas!.GetNearestNeibor(Position, AttachTag);
            if (AttachStart_CursorPosition.HasValue && (Vector)Mouse.GetPosition(RelativeCanvas) != AttachStart_CursorPosition.Value)
            {
                if (IsAttached)
                {
                    RoutedEventArgs args = new(UnAttachRoutedEvent, this);
                    RaiseEvent(args);
                    IsAttached = false;
                    return;
                }
                IsAttached = false;
                UnAttach();
                base.OnMouseMove(e);
                return;
            }
            if (!IsAttaching)
            {
                base.OnMouseMove(e);
            }
            else if (!IsAttached)
            {
                RoutedEventArgs args = new(AttachRoutedEvent, this);
                RaiseEvent(args);
            }
            if (AttachStart_CursorPosition is not null) return;
            double distance = (Position - AttachPoint)!.Value.Length;
            if (distance <= AttachRange)
            {
                Attach();
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (IsAttaching)
            {
                double distance = (Position - AttachPoint)!.Value.Length;
                if (distance < AttachMinThreshold)
                {
                    Canvas.SetLeft(this, AttachPoint!.Value.X);
                    Canvas.SetTop(this, AttachPoint!.Value.Y);
                    return;
                }
                UnAttach();
                base.OnMouseMove(e);
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (IsAttaching && !IsAttached)
            {
                UnAttach();
                base.OnMouseMove(e);
            }
        }

        private void AttachableControl_Loaded(object sender, RoutedEventArgs e)
        {
            RelativeCanvas = this.GetAncestor<AttachableCanvas>() ?? throw new InvalidOperationException("AttachableControl必须处于AttachableCanvas布局内");
        }

        public virtual void Attach()
        {
            AttachStoryboard = new();
            DoubleAnimation AttachXAnimation = new()
            {
                From = Canvas.GetLeft(this),
                To = AttachPoint!.Value.X,
                Duration = AttachDuration,
                EasingFunction = AttachEasingFunction
            };
            Storyboard.SetTargetProperty(AttachXAnimation, new(Canvas.LeftProperty));
            Storyboard.SetTarget(AttachXAnimation, this);
            AttachStoryboard.Children.Add(AttachXAnimation);
            DoubleAnimation AttachYAnimation = new()
            {
                From = Canvas.GetTop(this),
                To = AttachPoint!.Value.Y,
                Duration = AttachDuration,
                EasingFunction = AttachEasingFunction
            };
            Storyboard.SetTargetProperty(AttachYAnimation, new(Canvas.TopProperty));
            Storyboard.SetTarget(AttachYAnimation, this);
            AttachStoryboard.Children.Add(AttachYAnimation);
            AttachStoryboard.Completed += (object? sender, EventArgs e) =>
            {
                Canvas.SetLeft(this, AttachPoint!.Value.X);
                Canvas.SetTop(this, AttachPoint!.Value.Y);
                IsAttached = true;
                RoutedEventArgs args = new(AttachCompleteRoutedEvent, this);
                RaiseEvent(args);
            };
            StartTimer();
            AttachStart_CursorPosition = (Vector)Mouse.GetPosition((Canvas)Parent);
        }

        public virtual void UnAttach()
        {
            IsAttaching = false;
            IsAttached = false;
            AttachStoryboard?.Stop();
            AttachStoryboard = null;
            StopTimer();
            AttachStart_CursorPosition = null;
        }
    }
}
