using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Level_Editor))]
public class Level_Loader_Search : Editor
{
    string searchText = "";
    bool foldout = true;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("Search Object");
        searchText = EditorGUILayout.TextField(searchText);
        searchText = searchText.Trim().ToLower();
        Level_Editor editor = (Level_Editor)target;
        if (searchText != "")
        {
            int i = 0;
            foreach (var item in editor.objs)
            {
                if (item.obj.name.ToLower().StartsWith(searchText))
                {
                    EditorGUILayout.LabelField(i + " " + item.obj.name + " " + item.color.r * 255 + " " + item.color.g * 255 + " " + item.color.b * 255);
                }
                i++;
            }
        }
    }
}
