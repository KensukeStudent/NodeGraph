using System;

namespace NodeGraph
{
    [Serializable]
    public class StringProperty : NodePropertyBase
    {
        public const string ClassName = "StringProperty";

        public string Text = "";
    }
}
