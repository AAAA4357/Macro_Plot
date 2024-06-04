using System.Windows;

namespace Macro_Plot.Node
{
    /// <summary>
    /// 节点开始连接事件参数
    /// </summary>
    /// <param name="routedEvent">路由事件</param>
    /// <param name="source">事件源</param>
    /// <param name="sourceNode">开始连接的节点</param>
    public class NodeStartConnectEventArgs(RoutedEvent routedEvent, object source, Node sourceNode) : RoutedEventArgs(routedEvent, source)
    {
        /// <summary>
        /// 开始连接的节点
        /// </summary>
        public Node SourceNode { get; } = sourceNode;
    }
}
