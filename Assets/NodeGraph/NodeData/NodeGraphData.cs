using System;
using System.Linq;
using System.Collections.Generic;

namespace NodeGraph
{
    /// <summary>
    /// ノードグラフデータ
    /// </summary>
    [Serializable]
    public class NodeGraphData
    {
        /// <summary>
        /// ノードグラフ名
        /// </summary>
        public string GraphPath = "";

        /// <summary>
        /// ノードグラフId
        /// </summary>
        public string objectId = "";

        /// <summary>
        /// ノードデータリスト
        /// </summary>
        public List<NodeData> NodeDataList = new List<NodeData>();

        /// <summary>
        /// ポートデータリスト
        /// </summary>
        public List<PortData> PortDataList = new List<PortData>();

        public PropertyManager PropertyManager = new PropertyManager();

        public NodeGraphData(string graphPath)
        {
            GraphPath = graphPath;
            objectId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// ノードデータリストに存在する一致するオブジェクトIdを取得
        /// </summary>
        public NodeData GetNodeData(string nodeObjectId)
        {
            return NodeDataList.Where(x => x.ObjectId == nodeObjectId).FirstOrDefault();
        }

        /// <summary>
        /// ポートデータリストに存在する一致するノードオブジェクトIdを複数取得
        /// </summary>
        public List<PortData> GetPortDataList(string nodeObjectId)
        {
            return PortDataList.Where(x => x.NodeObjectId == nodeObjectId).ToList();
        }
    }
}