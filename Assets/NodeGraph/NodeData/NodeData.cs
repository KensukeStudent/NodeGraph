using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace NodeGraph
{
    /// <summary>
    /// ノードデータ
    /// </summary>
    [Serializable]
    public class NodeData
    {
        /// <summary>
        /// ノードオブジェクトId
        /// </summary>
        public string ObjectId = "";

        /// <summary>
        /// クラス名 / ノード生成時に使用 
        /// </summary>
        public string NodeClassName = "";

        public NodeDrawState DrawState;

        /// <summary>
        /// InputPort ObjectIdリスト
        /// </summary>
        public List<string> InputPortList = new List<string>();

        /// <summary>
        /// OutputPort ObjectIdリスト
        /// </summary>
        public List<string> OutputPortList = new List<string>();

        /// <summary>
        /// プロパティ情報
        /// </summary>
        public NodePropertyInfo PropertyInfo = null;

        public NodeData()
        {
            ObjectId = Guid.NewGuid().ToString();
        }

        public string GetPortObjectId(Direction direction, int portIndex)
        {
            return direction == Direction.Input ? InputPortList[portIndex] : OutputPortList[portIndex];
        }
    }
}