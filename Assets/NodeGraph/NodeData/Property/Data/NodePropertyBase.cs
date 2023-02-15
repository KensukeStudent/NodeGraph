using System;

namespace NodeGraph
{
    /// <summary>
    /// ノードプロパティベースクラス
    /// </summary>
    [Serializable]
    public class NodePropertyBase
    {
        public string ObjectId = "";

        public NodePropertyBase()
        {
            ObjectId = Guid.NewGuid().ToString();
        }
    }
}