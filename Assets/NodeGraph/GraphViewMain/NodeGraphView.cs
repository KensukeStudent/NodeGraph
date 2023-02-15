using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace NodeGraph
{
    /// <summary>
    /// ノードエディター用Viewクラス
    /// </summary>
    public class NodeGraphView : GraphView
    {
        private NodeGraphData nodeGraphData = null;

        public NodeGraphView(NodeGraphWindow window, NodeGraphData _nodeGraphData) : base()
        {
            // グラフビュー環境設定
            SettingsGraphView();
            // 検索ウィンドウ作成
            CreateSearchWindow(window);
            nodeGraphData = _nodeGraphData;
            // グラフビューのノード作成
            CreateNodeGraph();
        }

        /// <summary>
        /// グラフビューの環境設定
        /// </summary>
        private void SettingsGraphView()
        {
            // 親サイズに合わせて自動ストレッチ
            this.StretchToParentSize();
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            // マウスでドラッグ可能状態に設定
            this.AddManipulator(new SelectionDragger());
            // ホイール移動可能
            this.AddManipulator(new ContentDragger());
            // 左クリック長押し移動で範囲選択可能
            this.AddManipulator(new RectangleSelector());
            Insert(0, new GridBackground());
        }

        /// <summary>
        /// 右クリックで検索ウィンドウを作成
        /// </summary>
        private void CreateSearchWindow(NodeGraphWindow window)
        {
            // 右クリックで検索windowを表示
            var menuWindow = ScriptableObject.CreateInstance<NodeSearchMenuWindowProvider>();
            menuWindow.Initilize(window, this);
            nodeCreationRequest += context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), menuWindow);
            };
        }

        /// <summary>
        /// 取得したデータからノードグラフを作成
        /// </summary>
        private void CreateNodeGraph()
        {
            // ノードの生成
            // ポートの生成
            foreach (var node in nodeGraphData.NodeDataList)
            {
                Type nodeType = Type.GetType(node.NodeClassName);
                NodeBase nodeBase = (NodeBase)Activator.CreateInstance(nodeType);
                if (!string.IsNullOrEmpty(node.PropertyInfo.PropertyId))
                {
                    node.PropertyInfo.Property = nodeGraphData.PropertyManager.GetProperty(node.PropertyInfo);
                }
                nodeBase.Initilize(new ResistNodeInfo(node, nodeGraphData.GetPortDataList(node.ObjectId)));
                AddElement(nodeBase);
            }

            // エッジの生成
            foreach (NodeGraphPort port in GetAllPortData())
            {
                foreach (var edgeData in port.PortData.EdgeList)
                {
                    // 結合先のノードとポート取得
                    NodeBase edgeNode = nodes.Select(x => (NodeBase)x).Where(x => x.ObjectId == edgeData.NodeObjectId).FirstOrDefault();
                    NodeGraphPort edgePort = edgeNode.GetPort(edgeData.PortObjectId);
                    // 既に結合済みならリターン
                    if (port.IsEdgeConnection(edgePort) && edgePort.IsEdgeConnection(port))
                        continue;

                    var edge = new Edge()
                    {
                        input = port.direction == Direction.Input ? port : edgePort,
                        output = port.direction == Direction.Output ? port : edgePort
                    };
                    AddElement(edge);
                    edge.input.Connect(edge);
                    edge.output.Connect(edge);
                }
            }
        }

        /// <summary>
        /// 全ポートを取得
        /// </summary>
        private List<NodeGraphPort> GetAllPortData()
        {
            var tmp = nodes.Select(x => (NodeBase)x).Select(x => x.GetPortList());
            var portDataList = new List<NodeGraphPort>();
            foreach (var item in tmp)
            {
                portDataList.AddRange(item);
            }
            return portDataList;
        }

        /// <summary>
        /// 接続できるポートを定義
        /// </summary>
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            compatiblePorts.AddRange(ports.ToList().Where(port =>
            {
                // 同じノードには繋げない
                if (startPort.node == port.node)
                    return false;

                // Input同士、Output同士は繋げない
                if (port.direction == startPort.direction)
                    return false;

                // ポートの型が一致していない場合は繋げない
                if (port.portType != startPort.portType)
                    return false;

                return true;
            }));

            return compatiblePorts;
        }

        /// <summary>
        /// 現在のGraphViewをデータ化する
        /// </summary>
        public NodeGraphData CreateCurrentNodeGraphData()
        {
            var data = new NodeGraphData(nodeGraphData.GraphPath);
            data.objectId = nodeGraphData.objectId;

            foreach (var item in nodes)
            {
                var node = item as NodeBase;
                data.NodeDataList.Add(node.GetLatestNodeData());
                data.PortDataList.AddRange(node.GetPortDataList());
                data.PropertyManager.AddData(node.PropertyInfo);
            }
            return data;
        }

        public void Execute()
        {
            // // Root地点から発火
            // var rootPort = nodeGraphAsset.Root.NodeBase.outputContainer.Children().FirstOrDefault() as Port;
            // var rootEdge = rootPort.connections.FirstOrDefault();
            // if (rootEdge == null) return;

            // var currentNode = rootEdge.input.node as NodeBase;

            // // outputで繋がっているPortのEdgeからNodeがあれば発火
            // while (true)
            // {
            //     currentNode.Execute();

            //     var outputPort = currentNode.outputContainer.Children().FirstOrDefault() as Port;
            //     if (outputPort == null)
            //         break;
            //     var edge = outputPort.connections.FirstOrDefault();

            //     currentNode = edge.input.node as NodeBase;
            // }
        }
    }

}