using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace NodeGraph
{
    /// <summary>
    /// ノード発火地点
    /// </summary>
    public class RootNode : NodeBase
    {
        protected override string Title { get; set; } = "Root";

        public RootNode()
        {
            capabilities -= Capabilities.Deletable;
            this.CreateOutputPortHorizontalSingle(new ResistPortInfo { portName = "output", portIndex = 0, type = typeof(Port) });
        }

        public override void Execute()
        {
        }
    }
}