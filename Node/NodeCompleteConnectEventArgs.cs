using System.Windows;

namespace Macro_Plot.Node
{
    /// <summary>
    /// 节点完成连接事件参数
    /// </summary>
    /// <param name="routedEvent">路由事件</param>
    /// <param name="source">事件源</param>
    /// <param name="sourceNode">开始连接的节点</param>
    /// <param name="targetNode">完成连接的节点</param>
    public class NodeCompleteConnectEventArgs(RoutedEvent routedEvent, object source, Node sourceNode, Node targetNode) : RoutedEventArgs(routedEvent, source)
    {
        /// <summary>
        /// 开始连接的节点
        /// </summary>
        public Node SourceNode { get; } = sourceNode;

        /// <summary>
        /// 完成连接的节点
        /// </summary>
        public Node TargetNode { get; } = targetNode;
    }
}
