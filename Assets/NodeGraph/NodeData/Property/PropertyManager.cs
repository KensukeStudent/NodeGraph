using System;
using System.Linq;
using System.Collections.Generic;

namespace NodeGraph
{
    [Serializable]
    public class PropertyManager
    {
        public List<StringProperty> StringPropertyList = new List<StringProperty>();

        public void AddData(NodePropertyInfo info)
        {
            if (info == null)
                return;

            switch (info.ClassName)
            {
                case StringProperty.ClassName:
                    StringPropertyList.Add(info.Property as StringProperty);
                    break;

                default:
                    break;
            }
        }

        public NodePropertyBase GetProperty(NodePropertyInfo info)
        {
            return info.ClassName switch
            {
                StringProperty.ClassName => StringPropertyList.Where(x => x.ObjectId == info.PropertyId).FirstOrDefault(),
                _ => throw new Exception("")
            };
        }
    }
}