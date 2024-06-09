using System.Windows;

namespace Macro_Plot.Utils
{
    public static class ControlExtension
    {
        public static T? GetAncestor<T>(this FrameworkElement element) where T : DependencyObject
        {
            if (element.Parent is null) return default;
            else if (element.Parent.GetType().IsAssignableTo(typeof(T))) return (T)element.Parent;
            else if (element.Parent.GetType().IsAssignableTo(typeof(FrameworkElement))) return GetAncestor<T>((FrameworkElement)element.Parent);
            else return default;
        }
    }
}
