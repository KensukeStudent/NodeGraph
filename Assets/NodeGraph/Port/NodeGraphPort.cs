using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace NodeGraph
{
    /// <summary>
    /// ポート登録時の情報
    /// </summary>
    public struct ResistPortInfo
    {
        public string portName;

        public int portIndex;

        public Type type;

        public PortData PortData;

        public string parentNodeObjectId;
    }

    /// <summary>
    /// ノードグラフ用ポート拡張クラス
    /// </summary>
    public class NodeGraphPort : Port
    {
        private class DefaultEdgeConnectorListener : IEdgeConnectorListener
        {
            private GraphViewChange m_GraphViewChange;

            private List<Edge> m_EdgesToCreate;

            private List<GraphElement> m_EdgesToDelete;

            public DefaultEdgeConnectorListener()
            {
                m_EdgesToCreate = new List<Edge>();
                m_EdgesToDelete = new List<GraphElement>();
                m_GraphViewChange.edgesToCreate = m_EdgesToCreate;
            }

            public void OnDropOutsidePort(Edge edge, Vector2 position)
            {
            }

            public void OnDrop(GraphView graphView, Edge edge)
            {
                m_EdgesToCreate.Clear();
                m_EdgesToCreate.Add(edge);
                m_EdgesToDelete.Clear();
                if (edge.input.capacity == Capacity.Single)
                {
                    foreach (Edge connection in edge.input.connections)
                    {
                        if (connection != edge)
                        {
                            m_EdgesToDelete.Add(connection);
                        }
                    }
                }

                if (edge.output.capacity == Capacity.Single)
                {
                    foreach (Edge connection2 in edge.output.connections)
                    {
                        if (connection2 != edge)
                        {
                            m_EdgesToDelete.Add(connection2);
                        }
                    }
                }

                if (m_EdgesToDelete.Count > 0)
                {
                    graphView.DeleteElements(m_EdgesToDelete);
                }

                List<Edge> edgesToCreate = m_EdgesToCreate;
                if (graphView.graphViewChanged != null)
                {
                    edgesToCreate = graphView.graphViewChanged(m_GraphViewChange).edgesToCreate;
                }

                foreach (Edge item in edgesToCreate)
                {
                    graphView.AddElement(item);
                    edge.input.Connect(item);
                    edge.output.Connect(item);
                }
            }
        }

        public PortData PortData { private set; get; } = null;

        public string ObjectId => PortData.ObjectId;

        public string NodeObjectId => PortData.NodeObjectId;

        protected NodeGraphPort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type, ResistPortInfo info) : base(portOrientation, portDirection, portCapacity, type)
        {
            portName = info.portName;
            PortData = info.PortData == null ? new PortData(info.parentNodeObjectId, info.portIndex) : info.PortData;
        }

        public static NodeGraphPort Create(Direction direction, Capacity capacity, ResistPortInfo info)
        {
            DefaultEdgeConnectorListener listener = new DefaultEdgeConnectorListener();
            NodeGraphPort port = new NodeGraphPort(Orientation.Horizontal, direction, capacity, info.type, info)
            {
                m_EdgeConnector = new EdgeConnector<Edge>(listener)
            };
            port.AddManipulator(port.m_EdgeConnector);
            return port;
        }

        public override void Connect(Edge edge)
        {
            base.Connect(edge);
            NodeGraphPort port = GetPortByEdge(edge);
            PortData.AddEdgeData(port.NodeObjectId, port.ObjectId);
        }

        public override void Disconnect(Edge edge)
        {
            base.Disconnect(edge);
            NodeGraphPort port = GetPortByEdge(edge);
            PortData.RemoveEdgeData(port.ObjectId);
        }

        /// <summary>
        /// 入力されたポートが既に結合済みか
        /// </summary>
        public bool IsEdgeConnection(NodeGraphPort port)
        {
            if (direction == Direction.Input)
            {
                return connections.Any(x => x.output == port);
            }
            else
            {
                return connections.Any(x => x.input == port);
            }
        }

        private NodeGraphPort GetPortByEdge(Edge edge)
        {
            return direction switch
            {
                Direction.Input => edge.output as NodeGraphPort,
                Direction.Output => edge.input as NodeGraphPort,
                _ => throw new Exception("")
            };
        }
    }
}