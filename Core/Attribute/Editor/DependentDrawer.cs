#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.ProbeAdjustmentVolume;

namespace Hashira.Core.Attribute
{
    [CustomPropertyDrawer(typeof(DependentAttribute))]
    public class DependentDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DependentAttribute dependent = (DependentAttribute)attribute;

            float indent = dependent.indent;
            Rect lRect = new Rect(position.x, position.y, indent, position.height);

            Rect propertyRect = new Rect(
                position.x + indent,
                position.y,
                position.width - indent,
                position.height
            );

            EditorGUI.DrawRect(
                new Rect(position.x + indent / 2, position.y, 1, position.height / 2),
                Color.gray
            );

            EditorGUI.DrawRect(
                new Rect(position.x + indent / 2, position.y + position.height / 2, indent / 2, 1),
                Color.gray
            );

            // 프로퍼티 그리기
            EditorGUI.PropertyField(propertyRect, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
#endif