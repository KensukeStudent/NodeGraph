using System;
using System.Linq;
using System.Collections.Generic;

namespace NodeGraph
{
    /// <summary>
    /// ポートデータ
    /// </summary>
    [Serializable]
    public class PortData
    {
        /// <summary>
        /// ポートObjectId
        /// </summary>
        public string ObjectId = "";

        /// <summary>
        /// ポートに付随する親ノードObjectId
        /// </summary>
        public string NodeObjectId = "";

        /// <summary>
        /// ノードのポート生成番号
        /// </summary>
        public int PortIndex = -1;

        /// <summary>
        /// エッジに紐づけるノードObjectIdリスト
        /// </summary>
        public List<EdgeData> EdgeList = new List<EdgeData>();

        public PortData(string nodeObjectId, int portId)
        {
            ObjectId = Guid.NewGuid().ToString();
            NodeObjectId = nodeObjectId;
            PortIndex = portId;
        }

        public PortData(string objectId, int portId, List<EdgeData> edgeList)
        {
            ObjectId = objectId;
            PortIndex = portId;
            EdgeList = edgeList;
        }

        public void AddEdgeData(string nodeObjectId, string portObjectId)
        {
            if (EdgeList.Any(x => x.NodeObjectId == nodeObjectId && x.PortObjectId == portObjectId))
            {
                return;
            }
            EdgeList.Add(new EdgeData(nodeObjectId, portObjectId));
        }

        public void RemoveEdgeData(string portObjectId)
        {
            EdgeData edgeData = EdgeList.Where(x => x.PortObjectId == portObjectId).FirstOrDefault();
            if (edgeData == null)
                throw new Exception("エッジデータが存在しません");
            EdgeList.Remove(edgeData);
        }
    }
}