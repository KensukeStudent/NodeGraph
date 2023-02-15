using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NodeGraph
{
    /// <summary>
    /// ノードプロパティ情報
    /// </summary>
    [Serializable]
    public class NodePropertyInfo
    {
        public string ClassName = "";

        public string PropertyId = "";

        private NodePropertyBase property = null;

        public NodePropertyBase Property
        {
            set
            {
                property = value;
                PropertyId = property.ObjectId;
            }
            get => property;
        }

        public NodePropertyInfo(string className)
        {
            ClassName = className;
        }

        public T GetProperty<T>() where T : NodePropertyBase
        {
            return property as T;
        }
    }
}