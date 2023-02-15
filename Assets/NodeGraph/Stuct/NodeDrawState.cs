using System;
using UnityEngine;

namespace NodeGraph
{
    /// <summary>
    /// ノード描画情報
    /// </summary>
    [Serializable]
    public struct NodeDrawState
    {
        [SerializeField]
        private Rect m_Position;

        public Rect position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }
    }
}