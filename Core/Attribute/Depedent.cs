using UnityEngine;

namespace Hashira.Core.Attribute
{
    public class DependentAttribute : PropertyAttribute
    {
        public float indent = 20f; // 들여쓰기 크기

        public DependentAttribute(float indent = 20f)
        {
            this.indent = indent;
        }
    }
}
