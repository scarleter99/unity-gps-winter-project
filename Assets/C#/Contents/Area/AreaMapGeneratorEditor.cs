using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AreaMapGenerator))]
public class AreaMapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AreaMapGenerator generator = (AreaMapGenerator)target;

        EditorGUILayout.BeginHorizontal(); 
        if (GUILayout.Button("Generate Subtile"))
        {
            generator.Init(Define.AreaName.Snowfield);
            generator.GenerateSubtiles();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Sub+Maintile"))
        {
            generator.Init(Define.AreaName.Snowfield);
            generator.GenerateSubtiles();
            generator.GenerateMainTile();
        }
        EditorGUILayout.EndHorizontal();

    }
}