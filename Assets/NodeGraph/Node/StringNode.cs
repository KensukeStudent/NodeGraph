using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace NodeGraph
{
    /// <summary>
    /// 文字列出力用ノード
    /// </summary>
    [Title("Basic", "String")]
    public class StringNode : NodeBase
    {
        protected override string Title { get; set; } = "String";

        private TextField textFiled = null;
        public string Text => textFiled.value;

        public override void Initilize(ResistNodeInfo info)
        {
            base.Initilize(info);
            if (NodeData.PropertyInfo == null)
            {
                NodeData.PropertyInfo = new NodePropertyInfo(StringProperty.ClassName);
                NodeData.PropertyInfo.Property = new StringProperty();
            }

            this.CreateInputPortHorizontalSingle(new ResistPortInfo { portName = "input", portIndex = 0, type = typeof(Port) });
            this.CreateOutputPortHorizontalMulti(new ResistPortInfo { portName = "output", portIndex = 0, type = typeof(string) });
            textFiled = new TextField();
            textFiled.value = NodeData.PropertyInfo.GetProperty<StringProperty>().Text;
            mainContainer.Add(textFiled);
        }

        public override NodeData GetLatestNodeData()
        {
            NodeData.PropertyInfo.GetProperty<StringProperty>().Text = Text;
            return base.GetLatestNodeData();
        }

        public override void Execute()
        {

        }
    }
}