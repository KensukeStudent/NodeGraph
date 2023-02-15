using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace NodeGraph
{
    /// <summary>
    /// ノード登録時の情報
    /// </summary>
    public class ResistNodeInfo
    {
        public NodeData NodeData { private set; get; }

        public List<PortData> PortDataList { private set; get; }

        public ResistNodeInfo(NodeData nodeData, List<PortData> portDataList)
        {
            NodeData = nodeData;
            PortDataList = portDataList;
        }

        public PortData GetPortData(Direction direction, int portIndex)
        {
            if (PortDataList == null)
            {
                return null;
            }

            string portObjectId = NodeData.GetPortObjectId(direction, portIndex);
            return PortDataList.Where(x => x.PortIndex == portIndex && x.ObjectId == portObjectId).FirstOrDefault();
        }
    }

    /// <summary>
    /// ノードグラフ用ノード拡張クラス
    /// </summary>
    public abstract class NodeBase : Node
    {
        /// <summary>
        /// ノードタイトル
        /// </summary>
        protected abstract string Title { get; set; }

        /// <summary>
        /// ノード情報
        /// </summary>
        public ResistNodeInfo Info = null;

        public NodeData NodeData => Info.NodeData;

        public string ObjectId => NodeData.ObjectId;

        public NodePropertyInfo PropertyInfo => NodeData.PropertyInfo;

        /// <summary>
        /// TODO : NodeSearchMenuWindowProvider時にコンストラクターが通るのでコンストラクターでは定義しない
        /// </summary>
        public virtual void Initilize(ResistNodeInfo info = null)
        {
            title = Title;
            if (info == null)
            {
                Info = new ResistNodeInfo(new NodeData(), null);
                Info.NodeData.NodeClassName = GetType().ToString();
            }
            else
            {
                Info = info;
                base.SetPosition(NodeData.DrawState.position);
            }
        }

        /// <summary>
        /// ポートエレメント追加
        /// </summary>
        public void AddPortElement(Direction direction, NodeGraphPort port)
        {
            if (direction == Direction.Input)
            {
                inputContainer.Add(port);
            }
            else if (direction == Direction.Output)
            {
                outputContainer.Add(port);
            }
        }

        /// <summary>
        /// ポートデータの追加
        /// </summary>
        public void AddPortData(Direction direction, string portObjectId)
        {
            List<string> portList = direction switch
            {
                Direction.Input => NodeData.InputPortList,
                Direction.Output => NodeData.OutputPortList,
                _ => throw new Exception()
            };
            portList.Add(portObjectId);
        }

        public override void SetPosition(Rect newPos)
        {
            NodeData.DrawState.position = newPos;
            base.SetPosition(newPos);
        }

        /// <summary>
        /// inputとouputで定義されているポートリストを取得
        /// </summary>
        public List<NodeGraphPort> GetPortList()
        {
            List<VisualElement> containerList = new List<VisualElement>()
        {
            outputContainer,
            inputContainer
        };
            var portList = new List<NodeGraphPort>();
            foreach (var container in containerList)
            {
                foreach (var child in container.Children())
                {
                    if (child is NodeGraphPort)
                    {
                        portList.Add(child as NodeGraphPort);
                    }
                }
            }
            return portList;
        }

        public NodeGraphPort GetPort(string portObjectId)
        {
            return GetPortList().Where(x => x.ObjectId == portObjectId).FirstOrDefault();
        }

        /// <summary>
        /// ノードに付随する全ポートのPortDataリストを取得
        /// </summary>
        public List<PortData> GetPortDataList()
        {
            return GetPortList().Select(x => x.PortData).ToList();
        }

        /// <summary>
        /// データを最新化して渡す
        /// </summary>
        public virtual NodeData GetLatestNodeData()
        {
            return NodeData;
        }

        /// <summary>
        /// イベント発火
        /// </summary>
        public abstract void Execute();
    }
}