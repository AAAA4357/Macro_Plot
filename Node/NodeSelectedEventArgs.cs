using System.Windows;

namespace Macro_Plot.Node
{
    /// <summary>
    /// 节点选择事件参数
    /// </summary>
    /// <param name="routedEvent">路由事件</param>
    /// <param name="source">事件源</param>
    /// <param name="selectedNode">选择的节点</param>
    public class NodeSelectedEventArgs(RoutedEvent routedEvent, object source, Node selectedNode) : RoutedEventArgs(routedEvent, source)
    {
        /// <summary>
        /// 选择的节点
        /// </summary>
        public Node SelectedNode { get; } = selectedNode;
    }
}
