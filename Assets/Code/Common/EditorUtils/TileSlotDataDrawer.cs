using Code.Game.Configs;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Code.Common.EditorUtils
{
    [CustomPropertyDrawer(typeof(TileObjectData))]
    public class TileSlotDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var objectIdProp = property.FindPropertyRelative("ObjectId");
            var posProp = property.FindPropertyRelative("Position");

            string labelWithPos = $"Tile [{posProp.vector2IntValue.x}, {posProp.vector2IntValue.y}]";
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, new GUIContent(labelWithPos));
            objectIdProp.stringValue = EditorGUI.TextField(position, objectIdProp.stringValue);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
#endif