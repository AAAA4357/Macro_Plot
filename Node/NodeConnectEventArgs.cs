using System.Windows;

namespace Macro_Plot.Node
{
    /// <summary>
    /// 节点连接事件参数
    /// </summary>
    /// <param name="routedEvent">路由事件</param>
    /// <param name="source">事件源</param>
    /// <param name="sourceNode">开始连接的节点</param>
    /// <param name="cursorPosition">鼠标位置</param>
    public class NodeConnectEventArgs(RoutedEvent routedEvent, object source, Node sourceNode, Point cursorPosition) : RoutedEventArgs(routedEvent, source)
    {
        /// <summary>
        /// 开始连接的节点
        /// </summary>
        public Node SourceNode { get; } = sourceNode;

        /// <summary>
        /// 鼠标位置
        /// </summary>
        public Point CursorPosition { get; } = cursorPosition;
    }
}
