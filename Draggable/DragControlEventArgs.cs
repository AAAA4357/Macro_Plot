using System.Windows;

namespace Macro_Plot.Draggable
{
    /// <summary>
    /// 拖动控件事件参数
    /// </summary>
    /// <remarks>
    /// 主构造函数
    /// </remarks>
    /// <param name="routedEvent">路由事件</param>
    /// <param name="source">事件源</param>
    /// <param name="cursor_current">光标位置-相对当前控件</param>
    /// <param name="control">控件位置-相对父控件</param>
    public class DragControlEventArgs(RoutedEvent routedEvent, object source, Point cursor_current, Point control) : RoutedEventArgs(routedEvent, source)
    {
        /// <summary>
        /// 光标位置-相对父控件
        /// </summary>
        public Point CursorPoint_Parent { get; } = cursor_current + (Vector)control;

        /// <summary>
        /// 光标位置-相对当前控件
        /// </summary>
        public Point CursorPoint_Current { get; } = cursor_current;

        /// <summary>
        /// 控件位置
        /// </summary>
        public Point ControlPoint { get; } = control;
    }
}
