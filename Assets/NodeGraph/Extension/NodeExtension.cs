using UnityEditor.Experimental.GraphView;

namespace NodeGraph
{
    /// <summary>
    /// ノード拡張クラス
    /// </summary>
    public static class NodeExtension
    {
        private static NodeGraphPort CreatePort(this NodeBase node, Direction direction, Port.Capacity capacity, ResistPortInfo info)
        {
            info.parentNodeObjectId = node.ObjectId;
            info.PortData = node.Info.GetPortData(direction, info.portIndex);
            NodeGraphPort port = NodeGraphPort.Create(direction, capacity, info);
            node.AddPortElement(direction, port);
            if (info.PortData == null)
            {
                node.AddPortData(direction, port.ObjectId);
            }
            return port;
        }

        /// <summary>
        /// Input横&シングルポート作成
        /// </summary>
        public static NodeGraphPort CreateInputPortHorizontalSingle(this NodeBase node, ResistPortInfo info)
        {
            return CreatePort(node, Direction.Input, Port.Capacity.Single, info);
        }

        /// <summary>
        /// Input横&マルチポート作成
        /// </summary>
        public static NodeGraphPort CreateInputPortHorizontalMulti(this NodeBase node, ResistPortInfo info)
        {
            return CreatePort(node, Direction.Input, Port.Capacity.Multi, info);
        }

        /// <summary>
        /// Output横&シングルポート作成
        /// </summary>
        public static NodeGraphPort CreateOutputPortHorizontalSingle(this NodeBase node, ResistPortInfo info)
        {
            return CreatePort(node, Direction.Output, Port.Capacity.Single, info);
        }

        /// <summary>
        /// Output横&マルチポート作成
        /// </summary>
        public static NodeGraphPort CreateOutputPortHorizontalMulti(this NodeBase node, ResistPortInfo info)
        {
            return CreatePort(node, Direction.Output, Port.Capacity.Multi, info); ;
        }
    }
}
