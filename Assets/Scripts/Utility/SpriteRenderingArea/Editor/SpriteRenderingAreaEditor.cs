using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(SpriteRenderingArea))]
[CanEditMultipleObjects]
public class SpriteRenderingAreaEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SpriteRenderingArea targetedArea = (SpriteRenderingArea)target;
        List<string> excludedProperties = new List<string>() { "m_Script" };
        if (!targetedArea.keepNativeIfInBounds)
            excludedProperties.Add("nativeSize");
        if (!targetedArea.autoResize)
            excludedProperties.Add("defaultParentGlobalScale", "defaultAreaSize");
        DrawPropertiesExcluding(serializedObject, excludedProperties.ToArray());
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();
        if (GUILayout.Button("Normalize"))
            targetedArea.NormalizeSize();
    }
}
