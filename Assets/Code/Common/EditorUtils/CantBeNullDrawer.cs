#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Code.Common.EditorUtils
{
    [CustomPropertyDrawer(typeof(CantBeNullAttribute))]
    public class CantBeNullDrawer : PropertyDrawer
    {
        private const float _helpBoxHeight = 30f;
        private const float _padding = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var isNull = property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null;
            var baseHeight = EditorGUI.GetPropertyHeight(property, label, true);
            return isNull ? baseHeight + _helpBoxHeight + _padding : baseHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var baseHeight = EditorGUI.GetPropertyHeight(property, label, true);
            var propertyRect = new Rect(position.x, position.y, position.width, baseHeight);

            EditorGUI.PropertyField(propertyRect, property, label, true);

            if (property.propertyType != SerializedPropertyType.ObjectReference ||
                property.objectReferenceValue != null)
            {
                return;
            }
            Rect helpBoxRect = new Rect(position.x, position.y + baseHeight + _padding, position.width, _helpBoxHeight);
            EditorGUI.HelpBox(helpBoxRect, $"{label.text} should not be null", MessageType.Error);
        }
    }
}
#endif
