using System.Linq;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NodeGraph
{
    /// <summary>
    /// ログ用ノード
    /// </summary>
    [Title("Basic", "Log")]
    public class LogNode : NodeBase
    {
        protected override string Title { get; set; } = "Log";

        private Port inputString = null;

        public override void Initilize(ResistNodeInfo info)
        {
            base.Initilize(info);
            inputString = this.CreateInputPortHorizontalSingle(new ResistPortInfo { portName = "input", portIndex = 0, type = typeof(string) });
        }

        public override void Execute()
        {
            var edge = inputString.connections.FirstOrDefault();
            // 文字列として変換
            var node = edge.output.node as StringNode;

            if (node == null) return;

            Debug.Log(node.Text);
        }
    }

}