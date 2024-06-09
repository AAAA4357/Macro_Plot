using Macro_Plot.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Macro_Plot.Draggable
{
    public class AttachablePoints : Control
    {
        static AttachablePoints()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AttachablePoints), new FrameworkPropertyMetadata(typeof(AttachablePoints)));
        }

        public AttachablePoints()
        {
            Loaded += AttachablePoints_Loaded;
        }

        private void AttachablePoints_Loaded(object sender, RoutedEventArgs e)
        {
            RelativeCanvas = this.GetAncestor<AttachableCanvas>() ?? throw new InvalidOperationException("AttachableControl必须处于AttachableCanvas布局内");
            RelativeCanvas.RegisterPoints(AttachPoints, AttachTag);
        }

        public static readonly DependencyProperty AttachPointsProperty = DependencyProperty.Register("AttachPoints", typeof(PointCollection), typeof(AttachablePoints), new PropertyMetadata(new PointCollection()));

        public static readonly DependencyProperty AttachTagProperty = DependencyProperty.Register("AttachTag", typeof(string), typeof(AttachablePoints), new PropertyMetadata(string.Empty));

        public PointCollection AttachPoints
        {
            get { return (PointCollection)GetValue(AttachPointsProperty); }
            set { SetValue(AttachPointsProperty, value); }
        }

        public string AttachTag
        {
            get { return (string)GetValue(AttachTagProperty); }
            set { SetValue(AttachTagProperty, value); }
        }

        AttachableCanvas? RelativeCanvas;
    }
}
