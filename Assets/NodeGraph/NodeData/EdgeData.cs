using System;

namespace NodeGraph
{
    /// <summary>
    /// エッジデータ
    /// </summary>
    [Serializable]
    public class EdgeData
    {
        /// <summary>
        /// ノード接続先Id
        /// </summary>
        public string NodeObjectId = "";

        /// <summary>
        /// ノード接続先のポートオブジェクトId
        /// </summary>
        public string PortObjectId = "";

        public EdgeData(string nodeObjectId, string portObjectId)
        {
            NodeObjectId = nodeObjectId;
            PortObjectId = portObjectId;
        }
    }
}