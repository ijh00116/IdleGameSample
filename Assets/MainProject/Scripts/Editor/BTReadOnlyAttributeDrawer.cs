using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEditor;

namespace BlackTree.Common
{
    [CustomPropertyDrawer(typeof(BTReadOnlyAttribute))]
    public class BTReadOnlyAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false; // Disable fields
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true; // Enable fields
        }
    }

}
