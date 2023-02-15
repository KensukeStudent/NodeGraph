using System;

namespace NodeGraph
{
    // アトリビュート使用対象を宣言
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
                    AttributeTargets.Method | AttributeTargets.Class |
                    AttributeTargets.Event | AttributeTargets.Parameter |
                    AttributeTargets.ReturnValue)]
    public class TitleAttribute : Attribute
    {
        public string[] title;
        public TitleAttribute(params string[] title) { this.title = title; }
    }
}
